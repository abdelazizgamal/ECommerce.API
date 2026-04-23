namespace ECommerce.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public PaginationMetadata Metadata { get; set; } = new PaginationMetadata();
    }
}
