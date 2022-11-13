using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP12_RazorPage_EntityFramework.Areas.Admin.Pages.User
{
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class Index : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly MasterDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Index(UserManager<User> userManager, MasterDbContext context,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public const int ItemsPerPage = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }

        public int countPages { get; set; }
        public int totalUser { get; set; }

        public class UserAndRole : User
        {
            public string RoleName { get; set; }
        }

        public List<UserAndRole> Users { get; set; }
        [TempData] public string StatusMessage { get; set; }

        public async Task OnGet(string searchString)
        {
            // Users = await _userManager.Users.OrderBy(x=>x.UserName).ToListAsync();
            if (_context.Users != null)
            {
                totalUser = await _context.Users.CountAsync();
                countPages = (int)Math.Ceiling((double)totalUser / ItemsPerPage);

                if (currentPage < 1)
                    currentPage = 1;
                if (currentPage > countPages)
                    currentPage = countPages;


                var qr = from a in _context.Users
                    orderby a.UserName descending
                    select a;

                var userList = await _context.Users.OrderByDescending(a => a.UserName).ToListAsync();

                // if (!string.IsNullOrEmpty(searchString))
                // {
                //     Users = await _context.Users.Where(x => x.UserName.Contains(searchString)).ToListAsync();
                // }

                Users = await _context.Users.OrderBy(x => x.UserName)
                    .Skip((currentPage - 1) *
                          ItemsPerPage) // Ví dụ : Trang 1 bỏ đi 0 phần tử, trang 2 bỏ đi itemperpage phần tử
                    .Take(ItemsPerPage) // Lấy ra itemperpage phần tử 
                    .Select(x => new UserAndRole
                    {
                        Id = x.Id,
                        UserName = x.UserName
                    })
                    .ToListAsync();
                
                foreach (var user in Users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    user.RoleName = string.Join(",", roles);
                }
                
            }
        }

        public void OnPost() => RedirectToPage();
    }
}