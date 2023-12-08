namespace Contacts_Management_API.Models
{
    public class QueryResponseMultiple<T> : IResponse where T : new()
    {
        public QueryResponseMultiple()
        {
            this.Items = new List<T>();
        }

        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public long TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
