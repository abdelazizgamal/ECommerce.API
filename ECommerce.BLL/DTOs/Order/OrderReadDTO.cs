namespace ECommerce.BLL
{
    public record OrderReadDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Status { get; set; }
        public List<OrderItemReadDTO> Items { get; set; } = new();
    }
}
