namespace Contacts_Management_API.Models
{
    public class CommandResponse : IResponse
    {
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
