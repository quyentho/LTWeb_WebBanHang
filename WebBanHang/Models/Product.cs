using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Price")]
        public int UnitPrice { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        [Display(Name="Description")]
        public string Desc { get; set; }
        [Display(Name="Still supplied")]
        public bool Status { get; set; }
        [Display(Name="Category")]
        public int Cate_Id { get; set; }
        [ForeignKey("Cate_Id")]
        public virtual Category Category { get; set; }

    }
}
