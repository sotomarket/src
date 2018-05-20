using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class ReportSalesViewModel
    {
        public DateTime OrderDate { get; set; }
        public int OrdersCount { get; set; }
        public int SalesCount { get; set; }
        public decimal PotentialSum { get; set; }
        public decimal RealisedSum { get; set; }
        public int DeferredTransactionsCount { get; set; }
        public decimal DiscountSum { get; set; }
    }
}