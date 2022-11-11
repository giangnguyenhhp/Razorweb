using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly MasterDbContext _context;

        public IndexModel(MasterDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get; set; } = default!;

        public const int ItemsPerPage = 15;
        [BindProperty(SupportsGet = true,Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }


        public async Task OnGetAsync(string searchString)
        {
            if (_context.Articles != null)
            {
                var totalArticle = await _context.Articles.CountAsync();
                countPages = (int)Math.Ceiling((double)totalArticle / ItemsPerPage);

                if (currentPage < 1)
                    currentPage = 1;
                if (currentPage > countPages)
                    currentPage = countPages;


                var qr = from a in _context.Articles
                    orderby a.Created descending
                    select a;

                var articles = await _context.Articles.OrderByDescending(a => a.Created).ToListAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    Article = await _context.Articles.Where(x => x.Title.Contains(searchString)).ToListAsync();
                }
                else
                {
                    Article = await _context.Articles.OrderByDescending(x => x.Created)
                        .Skip((currentPage-1)*ItemsPerPage) // Ví dụ : Trang 1 bỏ đi 0 phần tử, trang 2 bỏ đi itemperpage phần tử
                        .Take(ItemsPerPage)  // Lấy ra itemperpage phần tử 
                        .ToListAsync();
                }
            }
        }
    }
}