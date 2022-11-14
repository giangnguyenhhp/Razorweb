using Microsoft.AspNetCore.Authorization;

namespace ASP12_RazorPage_EntityFramework.Security.Requirements;

public class CanUpdateArticlesRequirement : IAuthorizationRequirement
{
    public CanUpdateArticlesRequirement(int year=2022, int month=6, int day=1)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public int Year { get; set; } 
    public int Month { get; set; } 
    public int Day { get; set; } 
}