using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    public class SupplierController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new SmDbContext())
            {
                var list = db.Suppliers.Select(x => new SupplierViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Identifier = x.Identifier,
                    Description = x.Description
                }).OrderBy(x => x.Name).ToArray();

                return View(list);
            };
        }
        
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (var db = new SmDbContext())
            {
                var model = db.Suppliers.Select(x => new SupplierViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Identifier = x.Identifier,
                    Description = x.Description
                }).First(x => x.Id == Id);

                return View(model);
            };
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Edit(SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                using (var db = new SmDbContext())
                {
                    var entity = db.Suppliers.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new Supplier();
                        db.Suppliers.Add(entity);
                    }
                    entity.Name = model.Name;
                    entity.Identifier = model.Identifier;
                    entity.Address = model.Address;
                    entity.Description = model.Description;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (var db = new SmDbContext())
                {
                    var entity = db.Suppliers.First(x => x.Id == Id);
                    db.Suppliers.Remove(entity);
                    db.SaveChanges();

                    return SuccessJson();
                }
            }
            catch (Exception ex)
            {
                return FailJson(ex);
            }
        }
    }
}