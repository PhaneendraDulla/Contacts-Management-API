using Contacts_Management_API.Models;

namespace Contacts_Management_API.Handlers.QueryHandlers
{
    public interface IGetContactsQueryHandler
    {
        Task<IResponse> GetAllContacts();
        Task<IResponse> GetContactById(int Id);
        Task<IResponse> GetContacts(GetContactsQuery query);
    }
}
