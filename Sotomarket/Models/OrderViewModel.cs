using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class OrderViewModel
    {
        [Display(Name = "Номер заказа")]
        public int? Id { get; set; }

        [Display(Name ="Наименование клиента(ФИО)")]
        public string ClientName { get; set; }
        
        [Display(Name = "ИИН/БИН клиента")]
        public string ClientIdentifier { get; set; }

        [Display(Name = "Адрес клиента")]
        public string ClientAddress { get; set; }

        [Display(Name = "Доп. информация о клиенте")]
        public string ClientDescription { get; set; }

        [Display(Name ="Дата заказа")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Оператор")]
        public virtual string Operator { get; set; }

        public bool HasSale { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }

    public class OrderItemViewModel
    {
        public int? Id { get; set; }

        public int pos { get; set; }

        public int GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string GoodsBrand { get; set; }

        public string GoodsCategory { get; set; }

        public string GoodsSubCategory { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }
    }
}