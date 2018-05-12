using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Models
{
    [Authorize(Roles ="Администратор,Директор,Менеджер")]
    public class IncomeController : Controller
    {
        // GET: Income
        public ActionResult Index(int page = 0, string search = "")
        {
            using (var db = new SmDbContext())
            {
                var list = db.Incomes.Select(x => new IncomeViewModel
                {
                    
                });

                return View();
            }
        }
        
        // GET: Income/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Income/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Income/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Income/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
