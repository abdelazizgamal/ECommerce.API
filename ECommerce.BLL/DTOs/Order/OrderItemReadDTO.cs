namespace ECommerce.BLL
{
    public record OrderItemReadDTO
    {
        public int ProductId { get; set; }
        public required string ProductTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
