
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL

{
    public record ProductEditDTO
    {
        public int Id { get; set; }

      
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? ImgUrl { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public int CategoryId { get; set; }

    }
}
