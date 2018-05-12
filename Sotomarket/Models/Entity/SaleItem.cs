using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class SaleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }

        public int OrderItemId { get; set; }
        
        public virtual OrderItem OrderItem { get; set; }

        public int GoodsId { get; set; }

        public virtual Goods Goods { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }
    }
}