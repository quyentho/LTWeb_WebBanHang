using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<OrderDetails> OrderDetails { get; set; }
        public ApplicationUser User { get; set; }
    }
}
