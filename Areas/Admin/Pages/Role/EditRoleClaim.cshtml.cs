using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class EditRoleClaim : RolePageModel
{
    public EditRoleClaim(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }

    public class InputModel
    {
        [DisplayName("Tên của Claim")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string? ClaimType { get; set; }

        [DisplayName("Giá trị của Claim")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string? ClaimValue { get; set; }
    }

    [BindProperty] public InputModel Input { get; set; }
    public IdentityRole? role { get; set; }
    public IdentityRoleClaim<string>? claim { get; set; }

    public async Task<IActionResult> OnGet(int? claimId)
    {
        if (claimId == null) return NotFound("Không tìm thấy Role");
        claim = await Context.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);
        if (claim == null) return NotFound("Không tìm thấy role");

        role = await RoleManager.FindByIdAsync(claim.RoleId);
        if (role == null) return NotFound("Không tìm thấy role");

        Input = new InputModel()
        {
            ClaimType = claim.ClaimType,
            ClaimValue = claim.ClaimValue
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? claimId)
    {
        if (claimId == null) return NotFound("Không tìm thấy Role");
        claim = await Context.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);
        if (claim == null) return NotFound("Không tìm thấy role");

        role = await RoleManager.FindByIdAsync(claim.RoleId);
        if (role == null) return NotFound("Không tìm thấy role");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Context.RoleClaims
            .Any(x =>
                x.RoleId == role.Id && x.ClaimType == Input.ClaimType && x.ClaimValue == Input.ClaimValue &&
                x.Id != claim.Id))
        {
            ModelState.AddModelError(string.Empty, "Claim đã có trong role");
            return Page();
        }

        claim.ClaimType = Input.ClaimType;
        claim.ClaimValue = Input.ClaimValue;

        await Context.SaveChangesAsync();

        StatusMessage = $"Vừa cập nhật claim";

        return RedirectToPage("./Edit", new { roleId = role.Id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int? claimId)
    {
        if (claimId == null) return NotFound("Không tìm thấy Role");
        claim = await Context.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);
        if (claim == null) return NotFound("Không tìm thấy claim");

        role = await RoleManager.FindByIdAsync(claim.RoleId);
        if (role == null) return NotFound("Không tìm thấy role");

        if (claim.ClaimType != null)
            if (claim.ClaimValue != null)
                await RoleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));

        StatusMessage = $"Vừa xóa claim";

        return RedirectToPage("./Edit", new { roleId = role.Id });
    }
}