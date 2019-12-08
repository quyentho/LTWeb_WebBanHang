using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Display(Name="Full Name")]
        public string Name { get; set; }
        public string Address { get; set; }


    }
}
