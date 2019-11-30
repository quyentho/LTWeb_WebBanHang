using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        [Required]
        public string Attribute { get; set; }
        [Required]
        public string Value { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
