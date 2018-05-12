using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sotomarket.Models
{
    public class SupplierViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
    }
}