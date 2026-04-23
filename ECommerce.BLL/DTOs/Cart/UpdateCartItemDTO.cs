namespace ECommerce.BLL
{
    public record UpdateCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
