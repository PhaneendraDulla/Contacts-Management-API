using Contacts_Management_API.Models;

namespace Contacts_Management_API.Handlers.QueryHandlers
{
    public interface IGetContactsQueryHandler
    {
        Task<List<Contact>> GetAllContacts();
        Task<Contact?> GetContactById(int Id);
    }
}
