using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    public class GoodsSubCategoryController : BaseController
    {
        public GoodsSubCategoryController()
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsCategories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).OrderBy(x => x.Text).ToList();

                list.Insert(0, new SelectListItem { Value = "", Text = " " });

                ViewBag.GoodsCategories = list;
            }
        }

        public ActionResult ListJson(int categoryId)
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsSubCategories
                    .Where(x=>x.GoodsCategoryId==categoryId)
                    .Select(x => new
                {
                    id=x.Id,
                    text = x.Name,
                });
                
                return Json(list.OrderBy(x => x.text).ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsSubCategories.Select(x => new GoodsSubCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    GoodsCategory = x.GoodsCategory.Name
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
                var model = db.GoodsSubCategories.Select(x => new GoodsSubCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    GoodsCategoryId = x.GoodsCategoryId
                }).First(x => x.Id == Id);

                return View(model);
            };
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Edit(GoodsSubCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                using (var db = new SmDbContext())
                {
                    var entity = db.GoodsSubCategories.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null)
                    {
                        entity = new GoodsSubCategory();
                        db.GoodsSubCategories.Add(entity);
                    }
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.GoodsCategoryId = model.GoodsCategoryId;
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
                    var entity = db.GoodsSubCategories.First(x => x.Id == Id);
                    db.GoodsSubCategories.Remove(entity);
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