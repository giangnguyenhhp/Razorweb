using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class Edit : RolePageModel
{
    public class InputModel
    {
        [DisplayName("Tên của role")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string? Name { get; set; }
    }
    [BindProperty] public InputModel Input { get; set; }
    public IdentityRole? Role { get; set; }

    public async Task<IActionResult> OnGetAsync(string? roleId)
    {
        if (roleId == null) return NotFound("Không tìm thấy role");

        Role = await RoleManager.FindByIdAsync(roleId);
        if (Role != null)
        {
            Input = new InputModel()
            {
                Name = Role.Name
            };
            return Page();
        }

        return NotFound("Không tìm thấy role");
    }
    public async Task<IActionResult> OnPostAsync(string? roleId)
    {
        if (roleId == null) return NotFound("Không tìm thấy role");
        Role = await RoleManager.FindByIdAsync(roleId);
        if(Role==null) return NotFound("Không tìm thấy role");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Role.Name = Input.Name;
        var result = await RoleManager.UpdateAsync(Role);

        if (result.Succeeded)
        {
            StatusMessage = $"Bạn vừa đổi tên : {Input.Name}";
            return RedirectToPage("./Index");
        }
        else
        {
            result.Errors.ToList().ForEach(er => { ModelState.AddModelError(string.Empty, er.Description); });
        }

        return Page();
    }

    public Edit(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }
}