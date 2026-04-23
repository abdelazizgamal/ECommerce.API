namespace ECommerce.BLL
{
    public record ProductReadDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }

        public int Count { get; set; }
        public string? Category { get; set; }

       
    }
}
