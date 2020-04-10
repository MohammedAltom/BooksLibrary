using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace BooksLibrary.Models.Account
{
    public class AccountViewModel
    {
        [Required]
        public string id { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public string concated_claims;



        public IList<IdentityRole> Roles { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
