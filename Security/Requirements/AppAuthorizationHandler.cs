using System.Security.Claims;
using ASP12_RazorPage_EntityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ASP12_RazorPage_EntityFramework.Security.Requirements;

public class AppAuthorizationHandler : IAuthorizationHandler
{
    private readonly ILogger<AppAuthorizationHandler> _logger;
    private readonly UserManager<User> _userManager;

    public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        // context.User đại diện cho User đang đăng nhập trên hệ thống
        // context.Resource HttpContext mặc định truyền tới
        // context.PendingRequirements các requirements chưa Xử lý

        var requirements = context.PendingRequirements.ToList();

        _logger.LogInformation("context.Resource ~ " + context.Resource?.GetType().Name);

        foreach (var requirement in requirements)
        {
            if (requirement is GenZRequirement)
            {
                // code xử lý kiểm tra User đảm bảo requirement/GenZRequirement
                if (IsGenZ(context.User, (GenZRequirement)requirement))
                {
                    context.Succeed(requirement);
                }
            }

            if (requirement is CanUpdateArticlesRequirement)
            {
                if (CanUpdateArticles(context.User, context.Resource, (CanUpdateArticlesRequirement)requirement))
                {
                    context.Succeed(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }

    private bool CanUpdateArticles(ClaimsPrincipal contextUser, object? contextResource,
        CanUpdateArticlesRequirement requirement)
    {
        if (contextUser.IsInRole("Admin"))
        {
            _logger.LogInformation("Admin cập nhật ...");
            return true;
        }

        var article = contextResource as Article;

        var dateCreated = article.Created;
        var dateCanUpdate = new DateTime(requirement.Year, requirement.Month, requirement.Day);
        if (dateCreated <= dateCanUpdate)
        {
            _logger.LogInformation("Quá ngày cập nhật");
            return false;
        }
        return true;
    }


    private bool IsGenZ(ClaimsPrincipal contextUser, GenZRequirement requirement)
    {
        var userTask = _userManager.GetUserAsync(contextUser);
        Task.WaitAll(userTask);
        var user = userTask.Result;

        if (user?.DateOfBirth == null)
        {
            _logger.LogInformation($"{user.UserName} không có năm sinh thỏa mãn GenZRequirement");
            return false;
        }

        int year = user.DateOfBirth.Value.Year;

        var success = year >= requirement.FromYear && year <= requirement.ToYear;

        if (success)
        {
            _logger.LogInformation($"{user.UserName} thỏa mãn GenZRequirement");
        }
        else
        {
            _logger.LogInformation($"{user.UserName} không thỏa mãn GenZRequirement");
        }

        return success;
        return true;
    }
}