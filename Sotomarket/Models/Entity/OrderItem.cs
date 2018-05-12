using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int GoodsId { get; set; }

        public virtual Goods Goods { get; set; }

        public int Amount { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}