using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class Income
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public DateTime IncomeDate { get; set; }

        public string DocumentNumber { get; set; }

        [Required]
        public string OperatorId { get; set; }

        public virtual AspNetUsers Operator { get; set; }

        public virtual ICollection<IncomeItem> IncomeItems { get; set; }
    }
}