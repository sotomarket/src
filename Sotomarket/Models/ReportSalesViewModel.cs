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
        public ReportSalesDetailsViewModel[] Details { get; set; }
    }

    public class ReportSalesDetailsViewModel
    {
        public string ClientName { get; set; }
        public string ShopAssistant { get; set; }
        public string GoodsName { get; set; }
        public decimal GoodsPrice { get; set; }
        public int OrderAmount { get; set; }
        public string Cashier { get; set; }
        public int? Amount { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
    }
}