namespace ECommerce.BLL
{
    public record CartReadDTO
    {
        public int Id { get; set; }
        public List<CartItemReadDTO> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
