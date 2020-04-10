using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BooksLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksLibrary.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RoleAddModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _RoleManager;

        public RoleAddModel(RoleManager<IdentityRole> roleManager)
        {
            _RoleManager = roleManager;
            roles = _RoleManager.Roles.ToList();
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<IdentityRole> roles = new List<IdentityRole>();

        public class InputModel
        {
            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Display(Name = "Role Claims:")]
            public string RoleClaims { get; set; }
        }

        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityRole irole = new IdentityRole
                {
                    Name = Input.Role
                };
                
                IdentityResult result = await _RoleManager.CreateAsync(irole);
                if (result.Succeeded)
                {
                    Logger.LogActivity("Role Has been successfully added");
                    if (Input.RoleClaims != "" && Input.RoleClaims != null)
                    {
                        string[] roleClaims = Input.RoleClaims.Split(',');
                        foreach (var rolecalime in roleClaims)
                        {

                            if (rolecalime != null && rolecalime != "")
                            {
                                System.Security.Claims.Claim claim = new System.Security.Claims.Claim(rolecalime , rolecalime);
                                var ClaimResult = await  _RoleManager.AddClaimAsync(irole, claim);
                                if (!ClaimResult.Succeeded)
                                {
                                    foreach (var error in ClaimResult.Errors)
                                    {
                                        ModelState.AddModelError(string.Empty, error.Description);
                                    }
                                    return LocalRedirect("/Identity/Account/RoleAdd");
                                }

                            }

                        }
                    }

                    ModelState.Clear();

                    return LocalRedirect("/Identity/Account/RoleAdd");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return LocalRedirect("/Identity/Account/RoleAdd");
        }

        public async Task<IActionResult> OnPostDelete(string RoleName)
        {
            IdentityRole Role = await _RoleManager.FindByNameAsync(RoleName);
            var claims = await _RoleManager.GetClaimsAsync(Role);
            if(claims != null)
            {
                    foreach(var claim in claims)
                {
                    var CDResult = await _RoleManager.RemoveClaimAsync(Role, claim);
                    if(!CDResult.Succeeded)
                    {
                        foreach (var error in CDResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return LocalRedirect("/Identity/Account/RoleAdd");
                    }
                }
            }
            var result = await _RoleManager.DeleteAsync(Role);
            if (result.Succeeded)
            {
                return LocalRedirect("/Identity/Account/RoleAdd");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return LocalRedirect("/Identity/Account/RoleAdd");
        }
    }
}