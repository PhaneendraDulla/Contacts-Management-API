using Contacts_Management_API.Models;
using Newtonsoft.Json;

namespace Contacts_Management_API.Handlers.QueryHandlers
{
    public class GetContactsQueryHandler : IGetContactsQueryHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<GetContactsQueryHandler> _logger;

        public GetContactsQueryHandler(ILogger<GetContactsQueryHandler> logger, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            try
            {
                _logger.LogInformation("Processing GetAllContacts");

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");
                var jsonData = await File.ReadAllTextAsync(fullPath);
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    _logger.LogInformation("No data is present");
                    return new List<Contact>();
                }

                var contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonData);
                if (contacts == null || contacts.Count == 0)
                {
                    _logger.LogInformation("No data is present");
                    return new List<Contact>();
                }

                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<Contact>();
            }
        }

        public async Task<Contact?> GetContactById(int Id)
        {
            try
            {
                _logger.LogInformation("Processing GetContactById for Id: {0}", Id);

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");
                var jsonData = await File.ReadAllTextAsync(fullPath);
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    _logger.LogInformation("No data is present");
                    return null;
                }

                var contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonData);
                if (contacts == null || contacts.Count == 0)
                {
                    _logger.LogInformation("No data is present");
                    return null;
                }

                var contact = contacts.FirstOrDefault(x => x.Id == Id);

                return contact;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
