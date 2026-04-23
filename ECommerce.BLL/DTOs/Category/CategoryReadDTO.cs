namespace ECommerce.BLL

{
    public sealed record CategoryReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
