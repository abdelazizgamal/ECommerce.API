namespace ECommerce.BLL
{
    public record CreateOrderDTO
    {
        public string ShippingCountry { get; set; } = "Not Provided";
        public string ShippingCity { get; set; } = "Not Provided";
    }
}
