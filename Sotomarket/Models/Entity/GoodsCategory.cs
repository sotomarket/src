using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class GoodsCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Goods> Goods { get; set; }

        public virtual ICollection<GoodsSubCategory> GoodsSubCategories { get; set; }
    }
}