// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable


using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.User
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models;

    public class AddRoleModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager=roleManager;
        }
        

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public SelectList allRoles { get; set; }
        public User User { get; set; }
        
        [BindProperty]
        [DisplayName("Các Role gán cho User")]
        public string[] RoleNames { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không tìm thấy User");
            }

            User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return NotFound($"Không tìm thấy User, id = {id}");
            }

            RoleNames = (await _userManager.GetRolesAsync(User)).ToArray();

            var roleNames =await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không tìm thấy User");
            }
            User = await _userManager.FindByIdAsync(id);
            
            if (User == null)
            {
                return NotFound($"Không tìm thấy User, id = {id}");
            }
            
            //RoleNames
            var oldRoleNames = (await _userManager.GetRolesAsync(User)).ToArray();

            var deleteRoles = oldRoleNames.Where(r => r.Contains(r));
            var addRoles = RoleNames.Where(r => r.Contains(r));
            
            var roleNames =await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            var resulDelete = await _userManager.RemoveFromRolesAsync(User, deleteRoles);
            if (!resulDelete.Succeeded)
            {
                resulDelete.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty,er.Description);
                });
                return Page();
            }
            
            var resultAdd = await _userManager.AddToRolesAsync(User, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty,er.Description);
                });
                return Page();
            }
            
            StatusMessage = $"Vừa cập nhật role cho User: {User.UserName} thành công";

            return RedirectToPage("./Index");
        }
    }
}