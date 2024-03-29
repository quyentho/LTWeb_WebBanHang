﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        //private readonly UserStore<ApplicationUser> _store;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender
           /* UserStore<ApplicationUser> store*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            //_store = store;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            public string Address { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var fullName = user.Name; 
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var address = user.Address;

            Username = userName;

            Input = new InputModel
            {
                Name = fullName,
                Address = address,
                PhoneNumber = phoneNumber
            };

      //      IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var fullName = user.Name;
            if (Input.Name != fullName)
            {
                user.Name = Input.Name;
            }

            //       var email = await _userManager.GetEmailAsync(user);
            //if (Input.Name != name)
            //{
            //    var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
            //    if (!setEmailResult.Succeeded)
            //    {
            //        var userId = await _userManager.GetUserIdAsync(user);
            //        throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
            //    }
            //}

            var phoneNumber = user.PhoneNumber;
            if (Input.PhoneNumber != phoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
            }
            var address = user.Address;
            if (Input.Address != address )
            {
                user.Address = Input.Address;
            }

           await _userManager.UpdateAsync(user);
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            
            store.Context.SaveChanges();
           
        //    await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        //public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }


        //    var userId = await _userManager.GetUserIdAsync(user);
        //    var email = await _userManager.GetEmailAsync(user);
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var callbackUrl = Url.Page(
        //        "/Account/ConfirmEmail",
        //        pageHandler: null,
        //        values: new { userId = userId, code = code },
        //        protocol: Request.Scheme);
        //    await _emailSender.SendEmailAsync(
        //        email,
        //        "Confirm your email",
        //        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //    StatusMessage = "Verification email sent. Please check your email.";
        //    return RedirectToPage();
        //}
    }
}
