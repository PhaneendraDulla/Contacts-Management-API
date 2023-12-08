using Contacts_Management_API.Models;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public interface IAddContactCommandHandler
    {
        Task<IResponse> AddContact(Contact conact);
    }
}
