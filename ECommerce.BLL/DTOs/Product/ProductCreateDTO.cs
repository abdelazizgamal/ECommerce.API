namespace ECommerce.BLL

{
    public class ProductCreateDTO
    {
        
        public  string Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
        public string? ImgUrl { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public int CategoryId { get; set; }

    }
}
