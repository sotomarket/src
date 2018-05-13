using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class IncomeViewModel
    {
        public int? Id { get; set; }

        [Display(Name ="Поставщик")]
        public int SupplierId { get; set; }

        [Display(Name = "Поставщик")]
        public string Supplier { get; set; }

        [Display(Name = "Дата документа")]
        public DateTime IncomeDate { get; set; }

        [Required]
        [Display(Name = "Номер документа")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Оператор")]
        public virtual string Operator { get; set; }

        public bool? Processed { get; set; }

        public IEnumerable<IncomeItemViewModel> IncomeItems { get; set; }
    }

    public class IncomeItemViewModel
    {
        public int? Id { get; set; }

        public int pos { get; set; }

        public int GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string GoodsBrand { get; set; }

        public string GoodsCategory { get; set; }

        public string GoodsSubCategory { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}