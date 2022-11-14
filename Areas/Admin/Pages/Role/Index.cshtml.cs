using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

//Policy : Tao ra cac policy -> AllowEditRole
[Authorize(Policy = "AllowEditRole")]
[Authorize(Roles = "Admin")]
// [Authorize(Roles = "Admin")]     Phải có cả role Admin và Vip
// [Authorize(Roles = "Vip")]       Phải có cả role Admin và Vip
// [Authorize(Roles = "Admin,Vip")] một trong các role
public class Index : RolePageModel
{
    public Index(RoleManager<IdentityRole> roleManager, MasterDbContext context) : base(roleManager, context)
    {
    }
    
    public class Role : IdentityRole
    {
        public string[] RoleClaims { get; set; }
    }
    public List<Role> roles { get; set; }

    public async Task OnGet()
    {
       var roleList = await RoleManager.Roles.OrderBy(x=>x.Name).ToListAsync();
       roles = new List<Role>();
       foreach (var role in roleList)
       {
           var claims =await RoleManager.GetClaimsAsync(role);
           var claimString = claims.Select(x => x.Type + "=" + x.Value);
           var roleModel = new Role()
           {
               Name = role.Name,
               Id = role.Id,
               RoleClaims = claimString.ToArray()
           };
           roles.Add(roleModel);
       }
    }

    public void OnPost() => RedirectToPage();


}