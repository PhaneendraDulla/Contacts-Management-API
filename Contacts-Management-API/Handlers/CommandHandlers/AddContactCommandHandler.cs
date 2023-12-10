using Contacts_Management_API.Handlers.QueryHandlers;
using Contacts_Management_API.Models;
using Newtonsoft.Json;

namespace Contacts_Management_API.Handlers.CommandHandlers
{
    public class AddContactCommandHandler : IAddContactCommandHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AddContactCommandHandler> _logger;
        private readonly IGetContactsQueryHandler _getContactsQueryHandler;

        public AddContactCommandHandler(ILogger<AddContactCommandHandler> logger, IWebHostEnvironment webHostEnvironment, IGetContactsQueryHandler getContactsQueryHandler)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _getContactsQueryHandler = getContactsQueryHandler;
        }
        public async Task<IResponse> AddContact(Contact newContact)
        {
            var response = new CommandResponse();
            try
            {
                _logger.LogInformation("Processing AddContact");

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");

                var existingContactsResponse = await _getContactsQueryHandler.GetAllContacts() as QueryResponseMultiple<Contact>;
                var existingContacts = existingContactsResponse?.Items.ToList();
                if (existingContacts?.Count == 0)
                {
                    newContact.Id = 0;
                }
                else
                {
                    newContact.Id = (int)(existingContacts?.Max(x => x.Id) + 1);
                }

                existingContacts?.Add(newContact);
                var updatedJsonData = JsonConvert.SerializeObject(existingContacts, Formatting.Indented);
                File.WriteAllText(fullPath, updatedJsonData);

                _logger.LogInformation("Contact Created");
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
