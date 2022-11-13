using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class Delete : RolePageModel
{
    public IdentityRole? Role { get; set; }

    public async Task<IActionResult> OnGetAsync(string? roleId)
    {
        if (roleId == null) return NotFound("Không tìm thấy role");

        Role = await RoleManager.FindByIdAsync(roleId);
        if (Role == null)
        {
            return NotFound("Không tìm thấy role");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? roleId)
    {
        if (roleId == null) return NotFound("Không tìm thấy role");
        Role = await RoleManager.FindByIdAsync(roleId);
        if (Role == null) return NotFound("Không tìm thấy role");

        var result = await RoleManager.DeleteAsync(Role);

        if (result.Succeeded)
        {
            StatusMessage = $"Bạn vừa xóa role : {Role.Name}";
            return RedirectToPage("./Index");
        }
        else
        {
            result.Errors.ToList().ForEach(er => { ModelState.AddModelError(string.Empty, er.Description); });
        }

        return Page();
    }

    public Delete(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }
}