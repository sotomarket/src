using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
    }
}