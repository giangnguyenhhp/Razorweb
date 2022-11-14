using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.User;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

public class EditUserClaim : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly MasterDbContext _context;

    public EditUserClaim(UserManager<User> userManager, MasterDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public User? User { get; set; }
    public IdentityUserClaim<string>? Claims { get; set; }
    [TempData] public string StatusMessage { get; set; }

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

    public async Task<IActionResult> OnGetAsync(string userid)
    {
        if (string.IsNullOrEmpty(userid)) return NotFound($"Không tìm thấy User");

        User = await _userManager.FindByIdAsync(userid);

        if (User == null) return NotFound($"Không tìm thấy User, id = {userid}");

        return Page();
    }

    public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
    {
        if (claimid == null) return NotFound($"Không tìm thấy User");

        Claims = _context.UserClaims.FirstOrDefault(x => x.Id == claimid);
        if (Claims != null)
        {
            User = await _userManager.FindByIdAsync(Claims.UserId);
            if (User == null) return NotFound($"Không tìm thấy User, id = {Claims.UserId}");

            Input = new InputModel()
            {
                ClaimType = Claims.ClaimType,
                ClaimValue = Claims.ClaimValue
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddClaimAsync(string userid)
    {
        if (string.IsNullOrEmpty(userid)) return NotFound($"Không tìm thấy User");

        User = await _userManager.FindByIdAsync(userid);

        if (User == null) return NotFound($"Không tìm thấy User, id = {userid}");

        var claims = _context.UserClaims.Where(x => x.UserId == userid).ToList();

        if (claims.Any(x => x.ClaimType == Input.ClaimType && x.ClaimValue == Input.ClaimValue))
        {
            ModelState.AddModelError(string.Empty, "Đặc tính (claim) đã có trong user");
        }

        if (Input.ClaimType != null && Input.ClaimValue != null)

            await _userManager.AddClaimAsync(User, new Claim(Input.ClaimType, Input.ClaimValue));

        StatusMessage = $"Bạn vừa thêm đặc tính (claim) cho user : {User.UserName}";

        return RedirectToPage("./AddRole", new { id = User.Id });
    }

    public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
    {
        if (claimid == null) return NotFound($"Không tìm thấy User");
        Claims = _context.UserClaims.FirstOrDefault(x => x.Id == claimid);
        if (Claims != null)
        {
            User = await _userManager.FindByIdAsync(Claims.UserId);

            if (User == null) return NotFound($"Không tìm thấy User, id = {Claims.UserId}");

            if (!ModelState.IsValid) return Page();

            var claims = _context.UserClaims.Where(x => x.Id == claimid).ToList();

            if (claims.Any(x => x.UserId == User.Id 
                                && x.Id != Claims.Id 
                                && x.ClaimType == Input.ClaimType 
                                && x.ClaimValue == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính (claim) đã có ");
                return Page();
            }

            Claims.ClaimType = Input.ClaimType;
            Claims.ClaimValue = Input.ClaimValue;
            
            await _context.SaveChangesAsync();
            StatusMessage = "Bạn vừa cập nhật Claim";
            return RedirectToPage("./AddRole", new { id = User.Id });
        }
        StatusMessage = $"Bạn vừa thêm đặc tính (claim) cho user : {User.UserName}";
        return RedirectToPage("./AddRole", new { id = User.Id });
    }
    
    public async Task<IActionResult> OnPostDeleteClaimAsync(int? claimid)
    {
        if (claimid == null) return NotFound($"Không tìm thấy User");
        Claims = _context.UserClaims.FirstOrDefault(x => x.Id == claimid);
        if (Claims != null)
        {
            User = await _userManager.FindByIdAsync(Claims.UserId);

            if (User == null) return NotFound($"Không tìm thấy User, id = {Claims.UserId}");
            
            await _userManager.RemoveClaimAsync(User, new Claim(Claims.ClaimType, Claims.ClaimValue));
            
        }
        StatusMessage = "Bạn vừa xóa Claim";
        return RedirectToPage("./AddRole", new { id = User.Id });
    }
}