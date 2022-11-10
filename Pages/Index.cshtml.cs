using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP12_RazorPage_EntityFramework.Pages;

//CRUD
public class IndexModel : PageModel
{
    private readonly MasterDbContext _dbContext;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, MasterDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        if (_dbContext.Articles != null)
        {
            var post = _dbContext.Articles.OrderByDescending(p => p.Created).ToList();
            ViewData["Post"] = post;
        }
    }
}