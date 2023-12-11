using System.ComponentModel.DataAnnotations;

namespace Contacts_Management_API.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "Id is required")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
    }
}
