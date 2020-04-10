using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BooksLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksLibrary.Areas.Identity.Pages.Account.Manage
{
    public class ProfileAdminModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public ProfileAdminModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _RoleManager = roleManager;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string AccountSatus { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string ID { get; set; }
            [Required]
            public string UserName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            public string Role { get; set; }
            public string ConcatedClaims { get; set; }

           public IEnumerable<IdentityRole> Roles;
           public Dictionary<string , string> UserClaims;
        }

        public async Task<IActionResult> OnGetAsync(String UserId  , String Update)
        {

            if(Update == "true")
            {
                StatusMessage = "User has been updated";
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserId}'.");
            }            
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var UserRoles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            string ConctClaims = "";
            foreach (var claim in claims)
            {
                ConctClaims = ConctClaims +  claim.Type + ",";
            }
            Dictionary<string, string> tempclaimstore = new Dictionary<string, string>();
            foreach(var claim in ClaimsStore.AllClaims)
            {

                if(ConctClaims.Contains(claim.Type + ","))
                {
                    tempclaimstore.Add(claim.Value, "checked='checked'");
                }
                else
                {
                    tempclaimstore.Add(claim.Value, "");
                }
            }

            Input = new InputModel
            {
                ID = UserId,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                Role = UserRoles.FirstOrDefault(),
                ConcatedClaims = ConctClaims,
                Roles = _RoleManager.Roles,
                UserClaims = tempclaimstore
            };

            var status = await _userManager.IsLockedOutAsync(user);
            if (status)
            {
                AccountSatus = "Unlock User";
            }
            else
            {
                AccountSatus = "lock User";
            }
            
            //IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(String UserID)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserID}'.");
            }

            var status = await _userManager.IsLockedOutAsync(user);
            if (status)
            {
                var LockRes = await _userManager.SetLockoutEnabledAsync(user, false);
                if (!LockRes.Succeeded)
                {
                    return NotFound($"Unable to Lock user with ID '{UserID}'.");
                }
            }
            else
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                var LockRes = await  _userManager.SetLockoutEndDateAsync(user , DateTime.Today.AddYears(10));
                if (!LockRes.Succeeded)
                {

                    return NotFound($"Unable to Lock user with ID '{UserID}'.");
                }
            }
            
            string url = "ProfileAdmin";
            return RedirectToPage(url, new { UserId = UserID, Update = "true" });

        }

        public async Task<IActionResult> OnPostDelete(String UserID)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserID}'.");
            }
            var DelRes = await _userManager.DeleteAsync(user);
            if (!DelRes.Succeeded)
            {
                return NotFound($"Unable to Delete user with ID '{UserID}'.");
            }
            string url = "~/Areas/Identity/Pages/Account/Accounts";
            return RedirectToPage(url);
        }

            public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.ID);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Input.ID}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
            // claim removal result
            
            var ClaimRemRes = await _userManager.RemoveClaimsAsync(user , await _userManager.GetClaimsAsync(user));
            if(!ClaimRemRes.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred removing Claims for user with ID '{userId}'.");
            }
            if (Input.ConcatedClaims!= null && Input.ConcatedClaims != "")
            {
                String[] NewClaims = Input.ConcatedClaims.Split(",");
                foreach (var StClaim in NewClaims)
                {

                    Claim Claim = ClaimsStore.AllClaims.FirstOrDefault(c => c.Type == StClaim);
                    if (Claim != null)
                    {
                        await _userManager.AddClaimAsync(user, Claim);
                    }
                }
            }

            var RoleRemResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!RoleRemResult.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred removing Roles for user with ID '{userId}'.");

            }

            var NewRoleResult = await _userManager.AddToRoleAsync(user, Input.Role);
            if (!NewRoleResult.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred Adding user to Roles , user with ID '{userId}'.");
            }



            //  await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            //    return RedirectToPage();
            var  userIdUrl = await _userManager.GetUserIdAsync(user);
            string url = "ProfileAdmin";
            return RedirectToPage(url , new { UserId = userIdUrl ,Update = "true" });
           // return Page();
        }
    }
}