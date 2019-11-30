﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;

namespace WebBanHang.Areas.Customer.Views.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var productList =await _db.Products.Include(m => m.Category).ToListAsync();
            return View(productList);
        }
        

        //Detail Get
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _db.Products.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
            return View(product);
        }

        //Detail Post
        //[HttpPost, ActionName("Detail")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DetailPost(int id)
        //{
        //    List<int> lstShoppingCart =
        //}
    }
}