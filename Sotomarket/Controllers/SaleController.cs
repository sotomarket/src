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
    public class SaleController : BaseController
    {
        // GET: Sale
        public ActionResult Index(int page = 0, string search = "")
        {
            ViewBag.searchValue = search;
            ViewBag.page = page;
            using (var db = new SmDbContext())
            {
                var list = db.Sales.Select(x => new SaleViewModel
                {
                    Id=x.Id,
                    ClientAddress=x.Order.ClientAddress,
                    ClientDescription=x.Order.ClientDescription,
                    ClientIdentifier=x.Order.ClientIdentifier,
                    ClientName=x.Order.ClientName,
                    OrderDate=x.Order.OrderDate,
                    RealisationDate=x.RealizationDate,
                    OrderId=x.OrderId,
                    Operator=x.Operator.Lastname + " " + x.Operator.Firstname,
                    Processed=x.Processed,
                    Paytype=x.Paytype,
                });

                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x => x.Id.ToString().Contains(search) 
                                    || x.Operator.Contains(search)
                                    || x.ClientAddress.Contains(search)
                                    || x.ClientDescription.Contains(search)
                                    || x.ClientIdentifier.Contains(search)
                                    || x.ClientName.Contains(search));
                }

                list=list.OrderByDescending(x=>x.RealisationDate).Skip(page * 50).Take(50);

                return View(list.ToArray());
            }
        }
        
        // GET: Sale/Create
        public ActionResult Create(int orderId)
        {
            using (var db = new SmDbContext())
            {
                var entity = db.Orders.First(x => x.Id == orderId);
                if (entity.Sales.Any())
                {
                    // По заказу оформлена продажа. Редирект на индекс
                    RedirectToAction("Index");
                }
                var model = new SaleViewModel
                {
                    OrderId = entity.Id,
                    ClientAddress = entity.ClientAddress,
                    ClientDescription = entity.ClientDescription,
                    OrderDate = entity.OrderDate,
                    ClientIdentifier = entity.ClientIdentifier,
                    ClientName = entity.ClientName,
                    Operator = entity.Operator.Lastname + " " + entity.Operator.Firstname,
                };

                model.SaleItems = db.OrderItems.Where(x => x.OrderId == orderId).Select(x => new SaleItemViewModel
                {
                    OrderItemId = x.Id,
                    Amount = x.Amount,
                    Price = x.Goods.Price,
                    GoodsId = x.GoodsId,
                    GoodsBrand = x.Goods.Brand,
                    GoodsCategory = x.Goods.GoodsCategory.Name,
                    GoodsName = x.Goods.Name,
                    GoodsSubCategory = x.Goods.GoodsSubCategory == null ? "" : x.Goods.GoodsSubCategory.Name
                }).ToArray();
                var pos = 0;
                foreach (var item in model.SaleItems)
                {
                    item.pos = pos++;
                }
                return View("Edit",model);
            }
        }
        

        // GET: Sale/Edit/5
        public ActionResult Edit(int id)
        {
            using(var db = new SmDbContext())
            {
                var entity = db.Sales.First(x => x.Id == id);
                if (entity.Processed)
                {
                    // Документ проведен, редактирование запрещено. Редирект на просмотр
                    RedirectToAction("Details", new { id });
                }
                var model = new SaleViewModel
                {
                    Id=entity.Id,
                    ClientIdentifier=entity.Order.ClientIdentifier,
                    ClientName=entity.Order.ClientName,
                    OrderId=entity.OrderId,
                    RealisationDate=entity.RealizationDate,
                    Paytype=entity.Paytype,
                    Operator=entity.Operator.Lastname + " " + entity.Operator.Firstname,
                };
                
                model.SaleItems = db.SaleItems.Where(x => x.SaleId == id).Select(x => new SaleItemViewModel
                {
                    Id=x.Id,
                    Amount=x.Amount,
                    Price=x.Price,
                    GoodsId=x.GoodsId,
                    GoodsBrand=x.Goods.Brand,
                    GoodsCategory=x.Goods.GoodsCategory.Name,
                    GoodsName=x.Goods.Name,
                    GoodsSubCategory=x.Goods.GoodsSubCategory==null ? "" : x.Goods.GoodsSubCategory.Name,
                    OrderItemId=x.OrderItemId,
                    Discount=x.Discount
                }).ToArray();

                var pos = 0;
                foreach (var item in model.SaleItems)
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
                var entity = db.Sales.First(x => x.Id == id);
                var model = new SaleViewModel
                {
                    Paytype=entity.Paytype,
                    Id = entity.Id,
                    ClientIdentifier = entity.Order.ClientIdentifier,
                    ClientName = entity.Order.ClientName,
                    OrderId = entity.OrderId,
                    RealisationDate = entity.RealizationDate,
                    ClientAddress=entity.Order.ClientAddress,
                    ClientDescription=entity.Order.ClientDescription,
                    Operator= entity.Operator.Lastname + " " + entity.Operator.Firstname,
                    OrderDate=entity.Order.OrderDate,
                };

                model.SaleItems = db.SaleItems.Where(x => x.SaleId == id).Select(x => new SaleItemViewModel
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Price = x.Price,
                    GoodsId = x.GoodsId,
                    GoodsBrand = x.Goods.Brand,
                    GoodsCategory = x.Goods.GoodsCategory.Name,
                    GoodsName = x.Goods.Name,
                    GoodsSubCategory = x.Goods.GoodsSubCategory == null ? "" : x.Goods.GoodsSubCategory.Name,
                    Discount=x.Discount,
                }).ToArray();
                return View(model);
            }
        }

        // POST: Sale/Edit/5
        [HttpPost]
        public ActionResult Edit(SaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var pos = 0;
                foreach (var item in model.SaleItems)
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
                    var entity = db.Sales.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new Sale();
                        db.Sales.Add(entity);
                    }
                    if (entity.Processed)
                    {
                        throw new Exception("Документ уже проведен!");
                    }
                    entity.OrderId = model.OrderId;
                    entity.Paytype = model.Paytype;
                    entity.OperatorId = db.AspNetUsers.First(x => x.UserName == User.Identity.Name).Id;
                    entity.RealizationDate = model.RealisationDate.Value;
                    entity.Processed = model.Processed??false;
                    db.SaveChanges();

                    var existedIds = model.SaleItems.Select(x => x.Id);
                    foreach(var item in db.SaleItems.Where(x => x.SaleId == model.Id && !existedIds.Contains(x.Id)))
                    {
                        db.SaleItems.Remove(item);
                    }

                    foreach(var modelItem in model.SaleItems)
                    {
                        var entityItem = db.SaleItems.FirstOrDefault(x => x.Id == modelItem.Id);
                        if (entityItem == null)
                        {
                            entityItem = new SaleItem { SaleId = entity.Id };
                            db.SaleItems.Add(entityItem);
                        }
                        entityItem.Discount = modelItem.Discount;
                        entityItem.OrderItemId = modelItem.OrderItemId;
                        entityItem.GoodsId = modelItem.GoodsId;
                        entityItem.Amount = modelItem.Amount;
                        entityItem.Price = modelItem.Price;
                        entityItem.Discount = modelItem.Discount;
                    }

                    db.SaveChanges();

                    // Если документ проведен, то обновим наличие на складе
                    if (model.Processed==true)
                    {
                        foreach(var item in model.SaleItems)
                        {
                            var goods = db.Goods.First(x => x.Id == item.GoodsId);
                            goods.Quantity -= item.Amount;
                        }
                        db.SaveChanges();
                    }

                    tr.Commit();

                    //var t = 0;
                    //var a = 5 / t;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Id", ex);
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

                    var entity = db.Sales.First(x => x.Id == id);
                    foreach(var item in db.SaleItems.Where(x=>x.SaleId==id))
                    {
                        db.SaleItems.Remove(item);
                    }
                    db.Sales.Remove(entity);
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
