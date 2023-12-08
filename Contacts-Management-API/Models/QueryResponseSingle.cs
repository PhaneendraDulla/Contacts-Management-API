namespace Contacts_Management_API.Models
{
    public class QueryResponseSingle<T> : IResponse where T : new()
    {       
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Item { get; set; }
    }
}
