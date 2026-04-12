namespace ECommerce.BLL.Entities;

public class ApplicationUser 
{
    public string FullName { get; set; }
    
    public string Country { get; set; }
    public string City { get; set; }

    // Navigation
    public Cart Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();


}