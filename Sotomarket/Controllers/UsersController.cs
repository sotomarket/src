using Sotomarket.Models;
using Sotomarket.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    //[Authorize(Roles ="Administrator")]
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index()
        {
            using (var db = new SmDbContext())
            {
                var users = db.AspNetUsers.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    Role = x.AspNetRoles.Select(r => r.Name).FirstOrDefault()
                }).ToList();

                return View(users);
            }
        }

        public ActionResult Create(string Id)
        {
            SetRoles();
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            UserViewModel model;
            using (var db = new SmDbContext())
            {
                var user = db.AspNetUsers.First(x => x.Id == Id);
                model = new UserViewModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Role = user.AspNetRoles.Select(x => x.Id).FirstOrDefault()
                };
            }
            SetRoles(model.Role);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id) && string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Требуется поле Пароль.");
            }

            if (!ModelState.IsValid)
            {
                SetRoles(model.Role);
                return View(model);
            }
            try
            {
                using (var db = new SmDbContext())
                {
                    var user = db.AspNetUsers.FirstOrDefault(x => x.Id == model.Id);
                    if (user == null)
                    {
                        user = new AspNetUsers { Id = Guid.NewGuid().ToString() };
                        db.AspNetUsers.Add(user);
                        user.UserName = model.UserName;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                    }
                    user.Email = model.Email;
                    user.Lastname = model.Lastname;
                    user.Firstname = model.Firstname;
                    user.PhoneNumber = model.PhoneNumber;
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var ph = new Microsoft.AspNet.Identity.PasswordHasher();
                        user.PasswordHash = ph.HashPassword(model.Password);
                    }
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        var role = db.AspNetRoles.FirstOrDefault(x => x.Id == model.Role);
                        if (role != null)
                        {
                            user.AspNetRoles.Clear();
                            user.AspNetRoles.Add(role);
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UserName", ex);

                SetRoles(model.Role);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            try
            {
                using (var db = new SmDbContext())
                {
                    var user = db.AspNetUsers.First(x => x.Id == Id);
                    db.AspNetUsers.Remove(user);
                    db.SaveChanges();

                    return SuccessJson();
                }
            }
            catch (Exception ex)
            {
                return FailJson(ex);
            }
        }



        private void SetRoles(string currentRole = "")
        {
            using (var db = new SmDbContext())
            {
                var roles = db.AspNetRoles.ToList();
                if (roles.Count() == 0)
                {
                    roles.Add(new AspNetRoles { Id = Guid.NewGuid().ToString(), Name = "Администратор" });
                    roles.Add(new AspNetRoles { Id = Guid.NewGuid().ToString(), Name = "Директор" });
                    roles.Add(new AspNetRoles { Id = Guid.NewGuid().ToString(), Name = "Менеджер" });
                    roles.Add(new AspNetRoles { Id = Guid.NewGuid().ToString(), Name = "Продавец" });
                    db.AspNetRoles.AddRange(roles);
                    db.SaveChanges();
                }
                ViewBag.Roles = roles.OrderBy(x => x.Name).Select(x => new SelectListItem { Value = x.Id, Text = x.Name, Selected = x.Id == currentRole }).ToList();
            }
        }
    }
}