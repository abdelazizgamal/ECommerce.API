namespace ECommerce.BLL
{
    public record CartItemReadDTO
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string? ImgUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
