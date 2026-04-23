namespace ECommerce.BLL
{
    public record AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
