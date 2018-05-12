using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    public class GoodsCategoryController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsCategories.Select(x => new GoodsCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).OrderBy(x => x.Name).ToArray();

                return View(list);
            };
        }

        public ActionResult ListJson()
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsCategories.Select(x => new 
                {
                    id=x.Id,
                    text = x.Name,
                    x.Description
                }).OrderBy(x => x.text).ToArray();
                return Json(new { results = list, pagination=new { more = false } }, JsonRequestBehavior.AllowGet);
            }
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
                var model = db.GoodsCategories.Select(x => new GoodsCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).First(x => x.Id == Id);

                return View(model);
            };
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Edit(GoodsCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                using (var db = new SmDbContext())
                {
                    var entity = db.GoodsCategories.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new GoodsCategory();
                        db.GoodsCategories.Add(entity);
                    }
                    entity.Name = model.Name;
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
                    var entity = db.GoodsCategories.First(x => x.Id == Id);
                    db.GoodsCategories.Remove(entity);
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