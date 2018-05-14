using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    [Authorize(Roles ="Администратор,Директор,Менеджер")]
    public class IncomeController : BaseController
    {
        // GET: Income
        public ActionResult Index(int page = 0, string search = "")
        {
            using (var db = new SmDbContext())
            {
                var list = db.Incomes.Select(x => new IncomeViewModel
                {
                    DocumentNumber=x.DocumentNumber,
                    Id=x.Id,
                    IncomeDate=x.IncomeDate,
                    Operator=x.Operator.Lastname + " " + x.Operator.Firstname,
                    Supplier=x.Supplier.Name,
                    Processed=x.Processed
                });

                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x => x.DocumentNumber.Contains(search) || x.Operator.Contains(search) || x.Supplier.Contains(search));
                }

                list=list.OrderByDescending(x=>x.IncomeDate).Skip(page * 50).Take(50);

                return View(list.ToArray());
            }
        }
        
        // GET: Income/Create
        public ActionResult Create()
        {
            using(var db= new SmDbContext())
            {
                ViewBag.Suppliers = db.Suppliers.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
            }
            return View("Edit");
        }
        

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {
            using(var db = new SmDbContext())
            {
                var entity = db.Incomes.First(x => x.Id == id);
                if (entity.Processed)
                {
                    // Документ проведен, редактирование запрещено. Редирект на просмотр
                    RedirectToAction("Details", new { id });
                }
                var model = new IncomeViewModel
                {
                    DocumentNumber=entity.DocumentNumber,
                    Id=entity.Id,
                    IncomeDate=entity.IncomeDate,
                    Operator=entity.Operator.Lastname + " " + entity.Operator.Firstname,
                    SupplierId=entity.SupplierId
                };
                
                model.IncomeItems = db.IncomeItems.Where(x => x.IncomeId == id).Select(x => new IncomeItemViewModel
                {
                    Id=x.Id,
                    Amount=x.Amount,
                    Price=x.Price,
                    GoodsId=x.GoodsId,
                    GoodsBrand=x.Goods.Brand,
                    GoodsCategory=x.Goods.GoodsCategory.Name,
                    GoodsName=x.Goods.Name,
                    GoodsSubCategory=x.Goods.GoodsSubCategory==null ? "" : x.Goods.GoodsSubCategory.Name
                }).ToArray();

                ViewBag.Suppliers = db.Suppliers.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
                var pos = 0;
                foreach (var item in model.IncomeItems)
                {
                    item.pos = pos++;
                }
                return View(model);
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new SmDbContext())
            {
                var entity = db.Incomes.First(x => x.Id == id);
                var model = new IncomeViewModel
                {
                    DocumentNumber = entity.DocumentNumber,
                    Id = entity.Id,
                    IncomeDate = entity.IncomeDate,
                    Operator = entity.Operator.Lastname + " " + entity.Operator.Firstname,
                    SupplierId = entity.SupplierId,
                    Supplier = entity.Supplier.Name,
                    Processed=entity.Processed
                };

                model.IncomeItems = db.IncomeItems.Where(x => x.IncomeId == id).Select(x => new IncomeItemViewModel
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Price = x.Price,
                    GoodsId = x.GoodsId,
                    GoodsBrand = x.Goods.Brand,
                    GoodsCategory = x.Goods.GoodsCategory.Name,
                    GoodsName = x.Goods.Name,
                    GoodsSubCategory = x.Goods.GoodsSubCategory == null ? "" : x.Goods.GoodsSubCategory.Name,
                }).ToArray();
                return View(model);
            }
        }

        // POST: Income/Edit/5
        [HttpPost]
        public ActionResult Edit(IncomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                using (var db = new SmDbContext())
                {
                    ViewBag.Suppliers = db.Suppliers.Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();
                }
                var pos = 0;
                foreach (var item in model.IncomeItems)
                {
                    item.pos = pos++;
                }
                return View(model);
            }

            try
            {
                using(var db=new SmDbContext())
                {
                    var tr = db.Database.BeginTransaction();
                    var entity = db.Incomes.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new Income();
                        db.Incomes.Add(entity);
                    }
                    if (entity.Processed)
                    {
                        throw new Exception("Документ уже проведен!");
                    }
                    entity.DocumentNumber = model.DocumentNumber;
                    entity.IncomeDate = model.IncomeDate;
                    entity.OperatorId = db.AspNetUsers.First(x => x.UserName == User.Identity.Name).Id;
                    entity.SupplierId = model.SupplierId;
                    entity.Processed = model.Processed??false;
                    db.SaveChanges();

                    var existedIds = model.IncomeItems.Select(x => x.Id);
                    foreach(var item in db.IncomeItems.Where(x => x.IncomeId == model.Id && !existedIds.Contains(x.Id)))
                    {
                        db.IncomeItems.Remove(item);
                    }

                    foreach(var modelItem in model.IncomeItems)
                    {
                        var entityItem = db.IncomeItems.FirstOrDefault(x => x.Id == modelItem.Id);
                        if (entityItem == null)
                        {
                            entityItem = new IncomeItem { IncomeId = entity.Id };
                            db.IncomeItems.Add(entityItem);
                        }
                        entityItem.GoodsId = modelItem.GoodsId;
                        entityItem.Amount = modelItem.Amount;
                        entityItem.Price = modelItem.Price;
                    }

                    db.SaveChanges();

                    // Если документ проведен, то обновим наличие на складе
                    if (model.Processed==true)
                    {
                        foreach(var item in model.IncomeItems)
                        {
                            var goods = db.Goods.First(x => x.Id == item.GoodsId);
                            goods.Quantity += item.Amount;
                        }
                        db.SaveChanges();
                    }

                    tr.Commit();

                    //var t = 0;
                    //var a = 5 / t;
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using(var db = new SmDbContext())
                {
                    var tr = db.Database.BeginTransaction();

                    var entity = db.Incomes.First(x => x.Id == id);
                    foreach(var item in entity.IncomeItems)
                    {
                        db.IncomeItems.Remove(item);
                    }
                    db.Incomes.Remove(entity);
                    db.SaveChanges();
                    tr.Commit();
                }

                return SuccessJson();
            }
            catch (Exception ex)
            {
                return FailJson(ex);
            }
        }
    }
}
