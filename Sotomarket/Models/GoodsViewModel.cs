using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class GoodsViewModel
    {
        [Display(Name="Наименование")]
        public string Name { get; set; }

        [Display(Name = "Бренд")]
        public string Brand { get; set; }

        public int? Id { get; set; }

        [Display(Name = "Номенклатурный код")]
        public string ProductCode { get; set; }

        [Display(Name = "Категория товара")]
        public int GoodsCategoryId { get; set; }

        [Display(Name = "Категория товара")]
        public string GoodsCategory { get; set; }

        [Display(Name = "Подкатегория товара")]
        public int? GoodsSubCategoryId { get; set; }

        [Display(Name = "Подкатегория товара")]
        public string GoodsSubCategory { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Количество на складе")]
        public int Quantity { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}