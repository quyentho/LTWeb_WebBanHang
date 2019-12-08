using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Data;
using WebBanHang.Models.ViewModels;
using WebBanHang.Extensions;
using WebBanHang.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public IList<OrderDetails> OrderDetails { get; set; }
        public ShoppingCartController(ApplicationDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

            OrderDetails = new List<OrderDetails>();
       
        }
        public IActionResult Index()
        {
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            string userId = _userManager.GetUserId(HttpContext.User);

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;

             Order userOrdered = new Order()
            {
                DateCreate = DateTime.Now,
                UserId = userId,
     
                Total = 0
            };

            TempData.Put("Order", userOrdered);

 


            if (listShoppingCart !=null)
            {
                foreach (int prodId in listShoppingCart)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        ProductId = prodId,
                        Product = _db.Products.Include(m=>m.Category).FirstOrDefault(p => p.Id == prodId),
                        OrderId = userOrdered.Id,
                        QuantityOrdered = 1
                    };
                    OrderDetails.Add(orderDetails);
              
                }
            }
            return View(OrderDetails);
        }
        [HttpPost,ActionName("Index")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult IndexPost()
        {
          List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            Order order = TempData.Get<Order>("Order");
            _db.Orders.Add(order);
            _db.SaveChanges();

            int total=0;
            foreach(var prodId in listShoppingCart)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = prodId,
                    Product = _db.Products.FirstOrDefault(m => m.Id == prodId),
                    OrderId = order.Id,
                    QuantityOrdered = OrderDetails.FirstOrDefault(o=>o.ProductId == prodId).QuantityOrdered
                    
                };

                _db.OrderDetails.Add(orderDetails);
                total += orderDetails.Product.UnitPrice * orderDetails.QuantityOrdered;
            }
            order.Total = total;
            _db.Orders.Update(order);
            _db.SaveChanges();

            listShoppingCart = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", listShoppingCart);

            return RedirectToAction("Index");
        }

        public IActionResult BuyConfirmation()
        {
            ShoppingCartViewModel shoppingCartVM = new ShoppingCartViewModel()
            {
                OrderDetails = _db.OrderDetails.Include(m=>m.Product).ToList(),
                User = _db.ApplicationUsers.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User))
            };
            return View(shoppingCartVM);
        }
        
        public IActionResult Remove(int id)
        {
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (listShoppingCart != null)
            {
                if (listShoppingCart.Contains(id))
                {
                    listShoppingCart.Remove(id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", listShoppingCart);
            return RedirectToAction(nameof(Index));
        }
        

    }
}