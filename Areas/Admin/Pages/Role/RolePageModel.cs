using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.Role;

public class RolePageModel : PageModel
{
    protected readonly RoleManager<IdentityRole> RoleManager;
    protected readonly MasterDbContext Context;
    [TempData]
    public string StatusMessage { get; set; }
    public RolePageModel(RoleManager<IdentityRole> roleManager, MasterDbContext context)
    {
        RoleManager = roleManager;
        Context = context;
    }
}