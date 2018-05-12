using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class Goods
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProductCode { get; set; }

        public int GoodsCategoryId { get; set; }

        public virtual GoodsCategory GoodsCategory { get; set; }

        public int? GoodsSubCategoryId { get; set; }

        public virtual GoodsSubCategory GoodsSubCategory { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public virtual ICollection<IncomeItem> IncomeItems { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}