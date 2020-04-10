using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BooksLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksLibrary.Areas.Identity.Pages.Account.Manage
{
    public class SetPassAdminModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SetPassAdminModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserID { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(String ID , String Set)
        {

            if(Set == "true")
            {
                StatusMessage = "User Pass has been Set";
            }
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{ID}'.");
            }

            Input = new InputModel
            {
                UserID = ID
            };
            /*  var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToPage("./ChangePassword");
            }*/

            return Page(); 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var user = await _userManager.FindByIdAsync(Input.UserID);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Input.UserID}'.");
            }

            var PassRemRes = await _userManager.RemovePasswordAsync(user);
            if (!PassRemRes.Succeeded)
            {
                return NotFound($"Error Getting rid of old user password for user with ID '{Input.UserID}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

          //  await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been set.";
            string url = "SetPassAdmin";
            return RedirectToPage(url, new { ID = Input.UserID , Set = "true" });

        }
    }
}