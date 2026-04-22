using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL

{
    public record CategoryEditDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
