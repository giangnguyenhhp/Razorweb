using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Pages.Blog
{
    [Authorize(Policy = "InGenZ")] // sinh nam tu 1997 den 2012
    public class DetailsModel : PageModel
    {
        private readonly MasterDbContext _context;

        public DetailsModel(MasterDbContext context)
        {
            _context = context;
        }

      public Article Article { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            else 
            {
                Article = article;
            }
            return Page();
        }
    }
}
