using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class AddRoleClaim : RolePageModel
{
    public AddRoleClaim(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }

    public class InputModel
    {
        [DisplayName("Tên của Claim")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string ClaimType { get; set; }

        [DisplayName("Giá trị của Claim")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string ClaimValue { get; set; }
    }

    [BindProperty] public InputModel Input { get; set; }
    public IdentityRole? role { get; set; }

    public async Task<IActionResult> OnGet(string roleId)
    {
        role = await RoleManager.FindByIdAsync(roleId);
        if (role == null) return NotFound("Không tìm thấy role");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string roleId)
    {
        role = await RoleManager.FindByIdAsync(roleId);
        if (role == null) return NotFound("Không tìm thấy role");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if ((await RoleManager.GetClaimsAsync(role)).Any(x => x.Type == Input.ClaimType && x.Value == Input.ClaimValue))
        {
            ModelState.AddModelError(string.Empty, "Claim đã có trong role");
            return Page();
        }

        var newClaims = new Claim(Input.ClaimType, Input.ClaimValue);
        var result = await RoleManager.AddClaimAsync(role, newClaims);
        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(er => { ModelState.AddModelError(string.Empty, er.Description); });
            return Page();
        }

        StatusMessage = $"Vừa thêm đặc tính (claim) mới cho vai trò (role) : {role.Name}";

        return RedirectToPage("./Edit", new { roleId = role.Id });
    }
}