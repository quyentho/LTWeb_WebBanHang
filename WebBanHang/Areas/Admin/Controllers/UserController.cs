using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    [Area("Admin")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        public async Task<IActionResult> Edit(string id)
        {

            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }
            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ClaimsPrincipal curUser = this.User;
            var userId = _userManager.GetUserId(curUser);
            if (curUser.IsInRole(SD.EndUser) && userId != id)
                return NotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            ClaimsPrincipal curUser = this.User;
            var userId = _userManager.GetUserId(curUser);
            if (curUser.IsInRole(SD.EndUser) && userId != id)
                return NotFound();

            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = _db.ApplicationUsers.Find(id);
                userFromDb.Name = user.Name;
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.Address = user.Address;

                _db.SaveChanges();
                if (curUser.IsInRole(SD.Admin))
                    return RedirectToAction(nameof(Index));
                return RedirectToAction("Index","Home",new { area="Customer"});
            }

            return View(user);
        }
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }
            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize(Roles = SD.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string id)
        {

            ApplicationUser userFromDb = _db.ApplicationUsers.Find(id);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}