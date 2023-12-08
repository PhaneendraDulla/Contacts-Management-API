using Contacts_Management_API.Models;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public interface IUpdateContactCommandHandler
    {
        Task<IResponse> UpdateContact(Contact conact);
    }
}
