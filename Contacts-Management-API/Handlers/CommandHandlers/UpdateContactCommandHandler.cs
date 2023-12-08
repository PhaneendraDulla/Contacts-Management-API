using Contacts_Management_API.Handlers.QueryHandlers;
using Contacts_Management_API.Models;
using Newtonsoft.Json;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public class UpdateContactCommandHandler : IUpdateContactCommandHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UpdateContactCommandHandler> _logger;
        private readonly IGetContactsQueryHandler _getContactsQueryHandler;

        public UpdateContactCommandHandler(ILogger<UpdateContactCommandHandler> logger, IWebHostEnvironment webHostEnvironment, IGetContactsQueryHandler getContactsQueryHandler)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _getContactsQueryHandler = getContactsQueryHandler;
        }
        public async Task<IResponse> UpdateContact(Contact contact)
        {
            var response = new CommandResponse();
            try
            {
                _logger.LogInformation("Processing UpdateContact");

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");

                var existingContactsResponse = await _getContactsQueryHandler.GetAllContacts() as QueryResponseMultiple<Contact>;
                var existingContacts = existingContactsResponse?.Items.ToList();
                var contactToUpdate = existingContacts?.FirstOrDefault(c => c.Id == contact.Id);
                if (contactToUpdate == null)
                {
                    _logger.LogInformation("No contact found with given Id");
                    response.ErrorMessage = "No contact found with given Id";
                    response.ErrorCode = -1;
                    return response;
                }

                contactToUpdate.FirstName = contact.FirstName;
                contactToUpdate.LastName = contact.LastName;
                contactToUpdate.Email = contact.Email;

                var updatedJsonData = JsonConvert.SerializeObject(existingContacts, Formatting.Indented);
                File.WriteAllText(fullPath, updatedJsonData);

                _logger.LogInformation("Contact Updated");
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
