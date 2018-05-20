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
    public class ReportsController : Controller
    {
        [HttpGet]
        public ActionResult Employees()
        {
            using(var db = new SmDbContext())
            {
                var list = db.AspNetUsers.Select(x => new SelectListItem { Value=x.Id, Text=x.Lastname + " " + x.Firstname }).OrderBy(x=>x.Text).ToList();
                list.Insert(0, new SelectListItem { Value = string.Empty, Text = "Все" });
                ViewBag.Employees = list;
            }
            
            return View("EmployeesRequest");
        }

        [HttpPost]
        public ActionResult Employees(DateTime start, DateTime end, string employeeId="")
        {
            ViewBag.Start = start.ToShortDateString();
            ViewBag.End = end.ToShortDateString();
            using (var db = new SmDbContext())
            {
                var list = from u in db.AspNetUsers
                           join o in db.Orders on u.Id equals o.OperatorId
                           join oi in db.OrderItems on o.Id equals oi.OrderId
                           join g in db.Goods on oi.GoodsId equals g.Id

                           join s in db.Sales.Where(x => x.Processed) on o.Id equals s.OrderId into ss
                           from s in ss.DefaultIfEmpty()
                           join si in db.SaleItems on new { SaleId = s.Id, OrderItemId = oi.Id } equals new { si.SaleId, si.OrderItemId } into sii
                           from si in sii.DefaultIfEmpty()
                           from r in u.AspNetRoles.DefaultIfEmpty()
                           select new
                           {
                               EmployeeId=u.Id,
                               u.Lastname,
                               u.Firstname,
                               UserRole = r==null ? "" : r.Name,
                               o.ClientName,
                               o.ClientIdentifier,
                               o.OrderDate,
                               OrderAmount=oi.Amount,
                               Amount=(int?)si.Amount,
                               GoodsName=g.Name,
                               GoodsPrice=g.Price,
                               Price=(decimal?)si.Price,
                               Discount=(decimal?)si.Discount,
                           };
                list = list.Where(x => x.OrderDate >= start && x.OrderDate < end);
                if (!string.IsNullOrEmpty(employeeId))
                {
                    list = list.Where(x => x.EmployeeId == employeeId);
                }
                var result = list.ToArray().GroupBy(k => new
                {
                    k.Lastname,
                    k.Firstname,
                    k.UserRole,
                    k.OrderDate,
                    k.ClientIdentifier,
                    k.ClientName,
                }, (k, x) => new ReportEmployeeViewModel
                {
                    Lastname = k.Lastname,
                    Firstname = k.Firstname,
                    UserRole = k.UserRole,
                    OrderDate = k.OrderDate,
                    ClientIdentifier = k.ClientIdentifier,
                    ClientName = k.ClientName,

                    Total = x.Sum(s => s.Price ?? 0 * s.Amount ?? 0 - s.Discount ?? 0),
                    OrderDetails = x.Select(d => new ReportEmployeeDetailsViewModel
                    {
                        GoodsName = d.GoodsName,
                        GoodsPrice = d.GoodsPrice,
                        OrderAmount = d.OrderAmount,
                        Price = d.Price ?? 0,
                        Amount = d.Amount ?? 0,
                        Discount = d.Discount ?? 0,
                    })
                }).OrderBy(x => x.Lastname).ThenBy(x => x.Firstname).ThenBy(x => x.OrderDate);
                return View(result);
            }
        }

        [HttpGet]
        public ActionResult Sales()
        {
            return View("SalesRequest");
        }

        [HttpPost]
        public ActionResult Sales(DateTime start, DateTime end)
        {
            ViewBag.Start = start.ToShortDateString();
            ViewBag.End = end.ToShortDateString();
            using (var db = new SmDbContext())
            {
                var list = from u in db.AspNetUsers
                           join o in db.Orders on u.Id equals o.OperatorId
                           join oi in db.OrderItems on o.Id equals oi.OrderId
                           join g in db.Goods on oi.GoodsId equals g.Id

                           join s in db.Sales.Where(x => x.Processed) on o.Id equals s.OrderId into ss
                           from s in ss.DefaultIfEmpty()
                           join si in db.SaleItems on new { SaleId = s.Id, OrderItemId = oi.Id } equals new { si.SaleId, si.OrderItemId } into sii
                           from si in sii.DefaultIfEmpty()
                           from r in u.AspNetRoles.DefaultIfEmpty()
                           select new
                           {
                               OrderId=o.Id,
                               SaleId=(int?)s.Id,
                               EmployeeId = u.Id,
                               u.Lastname,
                               u.Firstname,
                               UserRole = r == null ? "" : r.Name,
                               o.ClientName,
                               o.ClientIdentifier,
                               o.OrderDate,
                               RealizationDate=(DateTime?)s.RealizationDate,
                               OrderAmount = oi.Amount,
                               Amount = (int?)si.Amount,
                               GoodsName = g.Name,
                               GoodsPrice = g.Price,
                               Price=(decimal?)si.Price,
                               Discount=(decimal?)si.Discount,
                           };
                list = list.Where(x => x.OrderDate >= start && x.OrderDate < end);

                var result = list.ToArray().GroupBy(k => k.OrderDate.Date,
                    (k, x) => new ReportSalesViewModel
                    {
                        OrderDate = k,
                        OrdersCount = x.Select(c => c.OrderId).Distinct().Count(),
                        SalesCount = x.Select(c => c.SaleId).Distinct().Count(),
                        PotentialSum = x.Sum(s => s.GoodsPrice * s.OrderAmount),
                        RealisedSum = x.Sum(s => s.Price ?? 0 * s.Amount ?? 0 - s.Discount ?? 0),
                        DiscountSum = x.Sum(s => s.Discount ?? 0),
                    DeferredTransactionsCount=x.Where(d=>d.RealizationDate.HasValue && d.RealizationDate.Value.Date!=d.OrderDate.Date).Select(d=>d.SaleId).Distinct().Count(),
                }).OrderBy(x=>x.OrderDate);
                return View(result);
            }
        }
    }
}