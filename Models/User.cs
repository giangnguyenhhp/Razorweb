using Microsoft.AspNetCore.Identity;

namespace ASP12_RazorPage_EntityFramework.Models;

public class User : IdentityUser
{
    public DateTimeOffset? DateOfBirth { get; set; }
}