using System.ComponentModel.DataAnnotations;

namespace ECommerce.APIs;

public class RegisterDto
{

    [Required]
    [MinLength(2)]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
