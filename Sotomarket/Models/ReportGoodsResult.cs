using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class ReportGoodsResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime dat { get; set; }
        public decimal? IncomeDaySum { get; set; }
        public int? IncomeDayAmount { get; set; }
        public decimal? ExpenseDaySum { get; set; }
        public int? ExpenseDayAmount { get; set; }
        public int? DatQuantity { get; set; }

    }
}