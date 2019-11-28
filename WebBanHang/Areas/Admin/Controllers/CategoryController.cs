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
    public class CategoryController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Categories.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Category category)
        {
            if (!ModelState.IsValid)
                return NotFound();
              _db.Add(category);
             await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));           
        }
        
        //Edit get
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = _db.Categories.FirstOrDefault(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
                return View(category);

            if (!ModelState.IsValid)
                return View(category);

            _db.Update(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Detail get
        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = _db.Categories.FirstOrDefault(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Delete Get
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = _db.Categories.FirstOrDefault(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Delete Post
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(int id, Category category)
        {
            _db.Remove(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}