using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class IncomeViewModel
    {
        public int? Id { get; set; }

        public int SupplierId { get; set; }

        public string Supplier { get; set; }

        public DateTime IncomeDate { get; set; }

        public string DocumentNumber { get; set; }

        public virtual string Operator { get; set; }

        public IEnumerable<IncomeItemViewModel> IncomeItems { get; set; }
    }

    public class IncomeItemViewModel
    {
        public int? Id { get; set; }

        public int GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string GoodsBrand { get; set; }

        public string GoodsCategory { get; set; }

        public string GoodsSubCategory { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}