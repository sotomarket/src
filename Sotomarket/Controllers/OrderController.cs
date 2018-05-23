using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    [Authorize(Roles ="Администратор,Директор,Менеджер,Продавец,Кассир")]
    public class OrderController : BaseController
    {
        // GET: Income
        public ActionResult Index(int page = 0, string search = "")
        {
            ViewBag.searchValue = search;
            ViewBag.page = page;
            using (var db = new SmDbContext())
            {
                var list = db.Orders.Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    ClientAddress = x.ClientAddress,
                    ClientName=x.ClientName,
                    ClientDescription = x.ClientDescription,
                    ClientIdentifier = x.ClientIdentifier,
                    Operator = x.Operator.Lastname + " " + x.Operator.Firstname,
                    HasSale = x.Sales.Any()
                });

                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x => x.Id.ToString().Contains(search) 
                                        || x.Operator.Contains(search) 
                                        || x.ClientIdentifier.Contains(search)
                                        || x.ClientDescription.Contains(search)
                                        || x.ClientName.Contains(search));
                }

                list=list.OrderByDescending(x=>x.OrderDate).Skip(page * 50).Take(50);

                return View(list.ToArray());
            }
        }
        
        // GET: Income/Create
        public ActionResult Create()
        {
            return View("Edit");
        }
        

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {
            using(var db = new SmDbContext())
            {
                var entity = db.Orders.First(x => x.Id == id);
                if (entity.Sales.Any())
                {
                    // По заказу оформлена продажа. Редирект на просмотр
                    RedirectToAction("Details", new { id });
                }
                var model = new OrderViewModel
                {
                    Id=entity.Id,
                    ClientAddress=entity.ClientAddress,
                    ClientDescription=entity.ClientDescription,
                    OrderDate=entity.OrderDate,
                    ClientIdentifier=entity.ClientIdentifier,
                    ClientName=entity.ClientName,
                    Operator=entity.Operator.Lastname + " " + entity.Operator.Firstname,
                };
                
                model.OrderItems = db.OrderItems.Where(x => x.OrderId == id).Select(x => new OrderItemViewModel
                {
                    Id=x.Id,
                    Amount=x.Amount,
                    Price=x.Goods.Price,
                    GoodsId=x.GoodsId,
                    GoodsBrand=x.Goods.Brand,
                    GoodsCategory=x.Goods.GoodsCategory.Name,
                    GoodsName=x.Goods.Name,
                    GoodsSubCategory=x.Goods.GoodsSubCategory==null ? "" : x.Goods.GoodsSubCategory.Name
                }).ToArray();
                var pos = 0;
                foreach (var item in model.OrderItems)
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
                var entity = db.Orders.First(x => x.Id == id);
                var model = new OrderViewModel
                {
                    Id = entity.Id,
                    ClientAddress = entity.ClientAddress,
                    ClientDescription = entity.ClientDescription,
                    OrderDate = entity.OrderDate,
                    ClientIdentifier = entity.ClientIdentifier,
                    ClientName = entity.ClientName,
                    Operator = entity.Operator.Lastname + " " + entity.Operator.Firstname,
                };

                model.OrderItems = db.OrderItems.Where(x => x.OrderId == id).Select(x => new OrderItemViewModel
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Price = x.Goods.Price,
                    GoodsId = x.GoodsId,
                    GoodsBrand = x.Goods.Brand,
                    GoodsCategory = x.Goods.GoodsCategory.Name,
                    GoodsName = x.Goods.Name,
                    GoodsSubCategory = x.Goods.GoodsSubCategory == null ? "" : x.Goods.GoodsSubCategory.Name
                }).ToArray();
                return View(model);
            }
        }

        // POST: Income/Edit/5
        [HttpPost]
        public ActionResult Edit(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var pos = 0;
                foreach (var item in model.OrderItems)
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
                    var entity = db.Orders.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new Order();
                        db.Orders.Add(entity);
                    }
                    if (entity.Sales.Any())
                    {
                        throw new Exception("По данному заказу уже оформлена продажа!");
                    }
                    entity.ClientAddress = model.ClientAddress;
                    entity.ClientDescription = model.ClientDescription;
                    entity.ClientIdentifier = model.ClientIdentifier;
                    entity.ClientName = model.ClientName;
                    entity.OrderDate = model.OrderDate;
                    entity.OperatorId = db.AspNetUsers.First(x => x.UserName == User.Identity.Name).Id;
                    db.SaveChanges();

                    var existedIds = model.OrderItems.Select(x => x.Id);
                    foreach(var item in db.OrderItems.Where(x => x.OrderId == model.Id && !existedIds.Contains(x.Id)))
                    {
                        db.OrderItems.Remove(item);
                    }

                    foreach(var modelItem in model.OrderItems)
                    {
                        var entityItem = db.OrderItems.FirstOrDefault(x => x.Id == modelItem.Id);
                        if (entityItem == null)
                        {
                            entityItem = new OrderItem { OrderId = entity.Id };
                            db.OrderItems.Add(entityItem);
                        }
                        entityItem.GoodsId = modelItem.GoodsId;
                        entityItem.Amount = modelItem.Amount;
                    }

                    db.SaveChanges();
                    
                    tr.Commit();
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

                    var entity = db.Orders.First(x => x.Id == id);
                    foreach(var item in db.OrderItems.Where(x=>x.OrderId==id))
                    {
                        db.OrderItems.Remove(item);
                    }
                    db.Orders.Remove(entity);
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
