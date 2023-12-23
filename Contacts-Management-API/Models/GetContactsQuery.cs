using System.ComponentModel.DataAnnotations;

namespace Contacts_Management_API.Models
{
    public class GetContactsQuery
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
    }
}
