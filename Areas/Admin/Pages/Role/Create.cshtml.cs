using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class Create : RolePageModel
{
    public void OnGet()
    {
    }

    public class InputModel
    {
        [DisplayName("Tên của role")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256,MinimumLength = 3,ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        public string Name { get; set; }
    }
    
    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var newRole = new IdentityRole(Input.Name);
        var result = await RoleManager.CreateAsync(newRole);

        if (result.Succeeded)
        {
            StatusMessage = $"Bạn vừa tạo Role mới : {Input.Name}";
            return RedirectToPage("./Index");
        }
        else
        {
            result.Errors.ToList().ForEach(er =>
            {
                ModelState.AddModelError(string.Empty,er.Description);
            });
        }
        
        return Page();
    }
    
    public Create(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }
}