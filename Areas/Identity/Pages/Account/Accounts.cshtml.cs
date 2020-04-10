using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BooksLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksLibrary.Areas.Identity.Pages.Account
{
    public class AccountsModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public AccountsModel(UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;
           accounts = _UserManager.Users;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IEnumerable<ApplicationUser> accounts ;

        public class InputModel
        {
            [Required]
            [Display(Name = "ID")]
            public string ID { get; set; }

        }

        public void OnGet()
        {
        }
    }
}