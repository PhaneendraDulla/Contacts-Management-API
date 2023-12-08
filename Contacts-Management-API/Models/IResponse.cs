namespace Contacts_Management_API.Models
{
    public interface IResponse
    {
        int ErrorCode { get; set; }
        string ErrorMessage { get; set; }
    }
}
