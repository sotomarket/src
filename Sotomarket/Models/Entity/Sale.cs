using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public DateTime RealizationDate { get; set; }

        public string Paytype { get; set; }

        [Required]
        public string OperatorId { get; set; }

        public virtual AspNetUsers Operator { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}