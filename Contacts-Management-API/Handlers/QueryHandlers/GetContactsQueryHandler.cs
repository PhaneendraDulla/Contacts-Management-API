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

        public async Task<IResponse> GetContacts(GetContactsQuery query)
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

                query.Page = string.IsNullOrEmpty(query.Page.ToString()) ? 1 : query.Page;
                query.ItemsPerPage = string.IsNullOrEmpty(query.ItemsPerPage.ToString()) ? 5 : query.ItemsPerPage;
                query.SortField = string.IsNullOrEmpty(query.SortField) ? "id" : query.SortField;
                query.SortOrder = string.IsNullOrEmpty(query.SortOrder) ? "asc" : query.SortOrder;

                var filteredContacts = contacts;

                if (!string.IsNullOrEmpty(query.Id))
                {
                    filteredContacts = filteredContacts.Where(c => c.Id.ToString().Contains(query.Id)).ToList();
                }
                if (!string.IsNullOrEmpty(query.FirstName))
                {
                    filteredContacts = filteredContacts.Where(c => c.FirstName.ToLower().Contains(query.FirstName.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(query.LastName))
                {
                    filteredContacts = filteredContacts.Where(c => c.LastName.ToLower().Contains(query.LastName.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(query.Email))
                {
                    filteredContacts = filteredContacts.Where(c => c.Email.ToLower().Contains(query.Email.ToLower())).ToList();
                }

                var propertyInfo = typeof(Contact).GetProperty(query.SortField);

                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {

                        filteredContacts = query.SortOrder == "asc" ?
                            filteredContacts.OrderBy(c => propertyInfo.GetValue(c, null)?.ToString()?.ToLower()).ToList()
                            : filteredContacts.OrderByDescending(c => propertyInfo.GetValue(c, null)?.ToString()?.ToLower()).ToList();

                    }
                    else
                    {

                        filteredContacts = query.SortOrder == "asc" ?
                            filteredContacts.OrderBy(c => propertyInfo.GetValue(c, null)).ToList()
                            : filteredContacts.OrderByDescending(c => propertyInfo.GetValue(c, null)).ToList();
                    }
                }

                //if (query.SortField.ToLower() == "id")
                //{
                //    filteredContacts = query.SortOrder == "asc" ?
                //        filteredContacts.OrderBy(c => c.Id).ToList()
                //        : filteredContacts.OrderByDescending(c => c.Id).ToList();
                //}
                //if (query.SortField.ToLower() == "firstname")
                //{
                //    filteredContacts = query.SortOrder == "asc" ?
                //    filteredContacts.OrderBy(c => c.FirstName.ToLowerInvariant()).ToList()
                //    : filteredContacts.OrderByDescending(c => c.FirstName.ToLowerInvariant()).ToList();
                //}
                //if (query.SortField.ToLower() == "lastname")
                //{
                //    filteredContacts = query.SortOrder == "asc" ?
                //    filteredContacts.OrderBy(c => c.LastName.ToLowerInvariant()).ToList()
                //    : filteredContacts.OrderByDescending(c => c.LastName.ToLowerInvariant()).ToList();
                //}
                //if (query.SortField.ToLower() == "email")
                //{
                //    filteredContacts = query.SortOrder == "asc" ?
                //    filteredContacts.OrderBy(c => c.Email.ToLowerInvariant()).ToList()
                //    : filteredContacts.OrderByDescending(c => c.Email.ToLowerInvariant()).ToList();
                //}

                var startIndex = (query.Page-1) * query.ItemsPerPage;
                var endIndex = Math.Min( startIndex + query.ItemsPerPage, filteredContacts.Count);
                var paginatedContacts = filteredContacts.Skip(startIndex).Take(endIndex - startIndex).ToList();

                _logger.LogInformation("Contacts retrieved");
                response.TotalItems = filteredContacts.Count;
                response.Items = paginatedContacts;
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
