using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ASP12_RazorPage_EntityFramework.Models;

public class User : IdentityUser
{
    [StringLength(400)]
    public string? HomeAddress { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
}