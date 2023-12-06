using Contacts_Management_API.Models;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public interface IDeleteContactCommandHandler
    {
        void DeleteContact(int Id);
    }
}
