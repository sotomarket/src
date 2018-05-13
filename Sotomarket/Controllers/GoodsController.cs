using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    public class GoodsController : BaseController
    {
        public GoodsController()
        {
            using (var db = new SmDbContext())
            {
                var list = db.GoodsCategories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).OrderBy(x => x.Text).ToList();

                ViewBag.GoodsCategories = list;
            }
        }

        // GET: Goods
        public ActionResult Index(bool hideZero=true, int page = 0, string search = "")
        {
            ViewBag.hideZero = hideZero;
            ViewBag.searchValue = search;
            return View(GetList(!hideZero, true, 50, page, search));
        }

        public ActionResult IndexPartial(bool hideZero = true, int page =0, string search = "")
        {
            ViewBag.hideZero = hideZero;
            ViewBag.searchValue = search;
            return PartialView(GetList(!hideZero, true, 50, page, search));
        }

        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Create()
        {
            using(var db = new SmDbContext())
            {
                var cat = (ViewBag.GoodsCategories as IEnumerable<SelectListItem>).FirstOrDefault()?.Value;
                List<SelectListItem> list;
                if (int.TryParse(cat, out int categoryId)) {
                    list = db.GoodsSubCategories.Where(x => x.GoodsCategoryId == categoryId).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    }).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    list = new List<SelectListItem>();
                }

                ViewBag.GoodsSubCategories = list;
            }
            
            return View("Edit");
        }

        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult CreatePartial()
        {
            using (var db = new SmDbContext())
            {
                var cat = (ViewBag.GoodsCategories as IEnumerable<SelectListItem>).FirstOrDefault()?.Value;
                List<SelectListItem> list;
                if (int.TryParse(cat, out int categoryId))
                {
                    list = db.GoodsSubCategories.Where(x => x.GoodsCategoryId == categoryId).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    }).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    list = new List<SelectListItem>();
                }

                ViewBag.GoodsSubCategories = list;
            }
            return PartialView("CreatePartial");
        }

        [HttpGet]
        [Authorize(Roles ="Администратор,Директор,Менеджер")]
        public ActionResult Edit(int id)
        {
            using(var db = new SmDbContext())
            {
                var entity = db.Goods.First(x => x.Id == id);
                var model = new GoodsViewModel
                {
                    Id = entity.Id,
                    Brand = entity.Brand,
                    Description = entity.Description,
                    GoodsCategoryId = entity.GoodsCategoryId,
                    GoodsSubCategoryId = entity.GoodsSubCategoryId,
                    Name = entity.Name,
                    Price = entity.Price,
                    ProductCode = entity.ProductCode,
                    Quantity = entity.Quantity
                };

                var list = db.GoodsSubCategories.Where(x=>x.GoodsCategoryId==entity.GoodsCategoryId).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).OrderBy(x => x.Text).ToList();

                ViewBag.GoodsSubCategories = list;

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult Edit(GoodsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                SaveModel(model);
                if (model.Quantity > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", new { hideZero = false });
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Name", ex);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Директор,Менеджер")]
        public ActionResult SaveAjax(GoodsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("CreatePartial",model);
            }
            try
            {
                SaveModel(model);
                return SuccessJson();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex);
                return PartialView("CreatePartial", model);
            }
        }

        private void SaveModel(GoodsViewModel model)
        {
            using(var db=new SmDbContext())
            {
                var entity = db.Goods.FirstOrDefault(x => x.Id == model.Id);
                if (entity == null)
                {
                    entity = new Goods();
                    db.Goods.Add(entity);
                }
                entity.Brand = model.Brand;
                entity.Description = model.Description;
                entity.GoodsCategoryId = model.GoodsCategoryId;
                entity.GoodsSubCategoryId = model.GoodsSubCategoryId;
                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.ProductCode = model.ProductCode;

                db.SaveChanges();
            }
        }

        private GoodsViewModel[] GetList(bool showZero, bool getCount, int limit = 10, int page = 0, string search = "")
        {
            using (var db = new SmDbContext())
            {
                var list = db.Goods.Select(x => new GoodsViewModel
                {
                    Brand = x.Brand,
                    Description = x.Description,
                    GoodsCategory = x.GoodsCategory.Name,
                    GoodsSubCategory = x.GoodsSubCategory == null ? "" : x.GoodsSubCategory.Name,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ProductCode = x.ProductCode,
                    Quantity = x.Quantity
                });
                if (!showZero)
                {
                    list = list.Where(x => x.Quantity > 0);
                }
                if (getCount)
                {
                    ViewBag.Total = list.Count();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x => x.Name.Contains(search) ||
                                x.Description.Contains(search) ||
                                x.GoodsSubCategory.Contains(search) ||
                                x.GoodsCategory.Contains(search) ||
                                x.Brand.Contains(search));
                }
                list = list.OrderBy(x=>x.Name).Skip(page * limit).Take(limit);
                return list.ToArray();
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
                    var entity = db.Goods.First(x => x.Id == Id);
                    db.Goods.Remove(entity);
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