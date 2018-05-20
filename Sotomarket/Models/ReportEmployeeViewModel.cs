using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class ReportEmployeeViewModel
    {
        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public string UserRole { get; set; }

        public decimal Total { get; set; }

        public DateTime OrderDate { get; set; }
        public string ClientName { get; set; }
        public string ClientIdentifier { get; set; }
        public IEnumerable<ReportEmployeeDetailsViewModel> OrderDetails { get; set; }
    }

    public class ReportEmployeeDetailsViewModel
    {
        public decimal Discount { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int OrderAmount { get; set; }
        public decimal GoodsPrice { get; set; }
        public string GoodsName { get; set; }
    }
}