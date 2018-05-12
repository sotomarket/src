using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class IncomeItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IncomeId { get; set; }

        public virtual Income Income { get; set; }

        public int GoodsId { get; set; }

        public virtual Goods Goods { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}