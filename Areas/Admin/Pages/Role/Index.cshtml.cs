using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

[Authorize(Roles = "Admin")]
// [Authorize(Roles = "Admin")]     Phải có cả role Admin và Vip
// [Authorize(Roles = "Vip")]       Phải có cả role Admin và Vip
// [Authorize(Roles = "Admin,Vip")] một trong các role

public class Index : RolePageModel
{
    public Index(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }
    
    public List<IdentityRole> Roles { get; set; }

    public async Task OnGet()
    {
       Roles = await RoleManager.Roles.OrderBy(x=>x.Name).ToListAsync();
    }

    public void OnPost() => RedirectToPage();


}