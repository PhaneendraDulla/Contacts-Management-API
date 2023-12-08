using Contacts_Management_API.Handlers.QueryHandlers;
using Contacts_Management_API.Models;
using Newtonsoft.Json;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public class DeleteContactCommandHandler : IDeleteContactCommandHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DeleteContactCommandHandler> _logger;
        private readonly IGetContactsQueryHandler _getContactsQueryHandler;

        public DeleteContactCommandHandler(ILogger<DeleteContactCommandHandler> logger, IWebHostEnvironment webHostEnvironment, IGetContactsQueryHandler getContactsQueryHandler)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _getContactsQueryHandler = getContactsQueryHandler;
        }
        public async Task<IResponse> DeleteContact(int Id)
        {
            var response = new CommandResponse();
            try
            {
                _logger.LogInformation("Processing DeleteContact");

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");

                var existingContactsResponse = await _getContactsQueryHandler.GetAllContacts() as QueryResponseMultiple<Contact>;
                var existingContacts = existingContactsResponse?.Items.ToList();
                var contactToDelete = existingContacts?.FirstOrDefault(c => c.Id == Id);
                if (contactToDelete == null)
                {
                    _logger.LogInformation("No contact found with given Id");
                    response.ErrorMessage = "No contact found with given Id";
                    response.ErrorCode = -1;
                    return response;
                }
               
                existingContacts?.Remove(contactToDelete);
                var updatedJsonData = JsonConvert.SerializeObject(existingContacts, Formatting.Indented);
                File.WriteAllText(fullPath, updatedJsonData);

                _logger.LogInformation("Contact Deleted");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.ErrorMessage = ex.Message;
                response.ErrorCode = -1;
                return response;
            }
        }
    }
}
