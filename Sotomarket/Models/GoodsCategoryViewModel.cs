using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class GoodsCategoryViewModel
    {
        public int? Id { get;  set; }

        [Display(Name="Наименование")]
        public string Name { get;  set; }

        [Display(Name="Описание")]
        public string Description { get;  set; }
    }
}