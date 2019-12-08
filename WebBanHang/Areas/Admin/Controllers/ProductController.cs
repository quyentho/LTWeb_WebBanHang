using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.Models.ModelViews;
using WebBanHang.Utility;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;
        [BindProperty]
        public ProductViewModel ProductVM { get; set; }
        public ProductController(ApplicationDbContext db,HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            ProductVM = new ProductViewModel()
            {
                Product = new Product(),
                Categories = _db.Categories.ToList()
            };

        }
        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.Category);
            return View(await products.ToListAsync());
        }


        //Create Get

        public IActionResult Create()
        {
            return View(ProductVM);
        }

        //Create Post
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if(!ModelState.IsValid)
            {
                return View(ProductVM);
            }

            _db.Products.Add(ProductVM.Product);
            await _db.SaveChangesAsync();

            // upload image
            string webRootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            string uploadFolder = Path.Combine(webRootPath, SD.ImageFolder);

            if (files.Count != 0)// has file
            {
               

                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploadFolder, ProductVM.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                ProductVM.Product.Image = @"\" + SD.ImageFolder + @"\" + ProductVM.Product.Id + extension;

            }
            else // not has image
            {

                var defaultImage = Path.Combine(uploadFolder, SD.DefaultImage);

                System.IO.File.Copy(defaultImage, Path.Combine( uploadFolder, ProductVM.Product.Id + ".jpg"));

                ProductVM.Product.Image = @"\" + SD.ImageFolder + @"\" + ProductVM.Product.Id + ".jpg";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Edit Get
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ProductVM.Product = _db.Products.Include(m=>m.Category).FirstOrDefault(p => p.Id == id);

            if (ProductVM.Product == null)
                return NotFound();

            return View(ProductVM);
        }

        //Edit Post
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            if(!ModelState.IsValid)
            {
                return View(ProductVM);
            }

            var webRootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            var productFromDb = _db.Products.FirstOrDefault(m => m.Id == id);

            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension_old = Path.GetExtension(productFromDb.Image);
                var extension_new = Path.GetExtension(files[0].FileName);
                if (System.IO.File.Exists(Path.Combine(uploads, productFromDb.Id + extension_old)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, productFromDb.Id + extension_old));
                }
                using (var fileStream = new FileStream(Path.Combine(uploads, ProductVM.Product.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                ProductVM.Product.Image = @"\" + SD.ImageFolder + @"\" + ProductVM.Product.Id + extension_new;
            }

            if (ProductVM.Product.Image != null)
            {
                productFromDb.Image = ProductVM.Product.Image;
            }

            productFromDb.Name = ProductVM.Product.Name;
            productFromDb.UnitPrice = ProductVM.Product.UnitPrice;
            productFromDb.Unit = ProductVM.Product.Unit;
            productFromDb.Quantity = ProductVM.Product.Quantity;
            productFromDb.Status = ProductVM.Product.Status;
            productFromDb.Desc = ProductVM.Product.Desc;
          
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            ProductVM.Product = await _db.Products.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (ProductVM.Product == null)
                return NotFound();

            return View(ProductVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            ProductVM.Product = await _db.Products.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (ProductVM.Product == null)
                return NotFound();

            return View(ProductVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var webRootPath = _hostingEnvironment.WebRootPath;
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var uploads = Path.Combine(webRootPath, SD.ImageFolder);
            var extension = Path.GetExtension(product.Image);

            if (System.IO.File.Exists(Path.Combine(uploads, product.Id + extension)))
            {
                System.IO.File.Delete(Path.Combine(uploads, product.Id + extension));
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}