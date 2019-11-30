using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models.ModelViews
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
