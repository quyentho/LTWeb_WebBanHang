using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductDetailsController : Controller
    {

        private readonly ApplicationDbContext _db;

        [BindProperty]
         public ProductDetails ProductDetailsBinding { get; set; }
        
        public ProductDetailsController(ApplicationDbContext db)
        {
            _db = db;
            ProductDetailsBinding = new ProductDetails();
           
        }
        public IActionResult Index(int? productId)
        {
            if (productId == null)
                return NotFound();
            var productFromDb = _db.Products.FirstOrDefault(p => p.Id == productId);
         
            if (productFromDb != null)
            {
                var productDetails = _db.ProductDetails.Where(m => m.ProductId == productId).ToList();
                ViewBag.ID = productId;
                return View(productDetails);
            }
            return NotFound();
           
        }

        public IActionResult Create(int? productId)
        {
            ProductDetailsBinding.ProductId =Convert.ToInt32(productId);
            return View(ProductDetailsBinding);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
        
            if (!ModelState.IsValid)
                return NotFound();

            _db.Add(ProductDetailsBinding);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index",new { productId = ProductDetailsBinding.ProductId });
        }

        //Edit get
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

           ProductDetailsBinding = _db.ProductDetails.FirstOrDefault(m => m.Id == id);

            if (ProductDetailsBinding == null)
            {
                return NotFound();
            }

            return View(ProductDetailsBinding);
        }

        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != ProductDetailsBinding.Id)
                return View(ProductDetailsBinding);

            if (!ModelState.IsValid)
                return View(ProductDetailsBinding);

            _db.Update(ProductDetailsBinding);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { productId = ProductDetailsBinding.ProductId });
        }

        //Detail get
        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();

            ProductDetailsBinding = _db.ProductDetails.FirstOrDefault(m => m.Id == id);

            if (ProductDetailsBinding == null)
            {
                return NotFound();
            }

            return View(ProductDetailsBinding);
        }

        //Delete Get
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            ProductDetailsBinding = _db.ProductDetails.FirstOrDefault(m => m.Id == id);

            if (ProductDetailsBinding == null)
            {
                return NotFound();
            }
           
            return View(ProductDetailsBinding);
        }

        //Delete Post
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(int id)
        {
            int productId = ProductDetailsBinding.ProductId;
            _db.Remove(ProductDetailsBinding);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId = ProductDetailsBinding.ProductId });
        }

    }
}