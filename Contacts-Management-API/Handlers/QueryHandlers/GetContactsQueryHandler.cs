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

        public async Task<IResponse> GetAllContacts()
        {
            var response = new QueryResponseMultiple<Contact>();
            try
            {
                _logger.LogInformation("Processing GetAllContacts");

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");
                var jsonData = await File.ReadAllTextAsync(fullPath);
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    _logger.LogInformation("No data is present");
                    response.ErrorMessage = "No data is present";
                    response.ErrorCode = -1;
                    return response;
                }

                var contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonData);
                if (contacts == null || contacts.Count == 0)
                {
                    _logger.LogInformation("No data is present");
                    response.ErrorMessage = "No data is present";
                    response.ErrorCode = -1;
                    return response;
                }

                _logger.LogInformation("Contacts retrieved");
                response.TotalItems = contacts.Count;
                response.Items = contacts;
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

        public async Task<IResponse> GetContactById(int Id)
        {
            var response = new QueryResponseSingle<Contact>();
            try
            {
                _logger.LogInformation("Processing GetContactById for Id: {0}", Id);

                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/ContactsData.json");
                var jsonData = await File.ReadAllTextAsync(fullPath);
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    _logger.LogInformation("No data is present");
                    response.ErrorMessage = "No data is present";
                    response.ErrorCode = -1;
                    return response;
                }

                var contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonData);
                if (contacts == null || contacts.Count == 0)
                {
                    _logger.LogInformation("No data is present");
                    response.ErrorMessage = "No data is present";
                    response.ErrorCode = -1;
                    return response;
                }

                var contact = contacts.FirstOrDefault(x => x.Id == Id);

                _logger.LogInformation("Contact retrieved");
               
                response.Item = contact;

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
