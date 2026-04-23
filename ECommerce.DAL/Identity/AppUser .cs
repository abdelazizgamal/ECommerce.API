
using ECommerce.Common;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL;

public class AppUser : IdentityUser, IAuditableEntity
{
    public string FullName { get; set; }

    public string? Country { get; set; }
    public string? City { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}