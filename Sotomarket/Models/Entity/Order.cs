using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sotomarket.Models.Entity
{
    public class Order
    {
        public Order()
        {
            Sales = new HashSet<Sale>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string ClientIdentifier { get; set; }

        public string ClientAddress { get; set; }

        public string ClientDescription { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public string OperatorId { get; set; }

        public virtual AspNetUsers Operator { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}