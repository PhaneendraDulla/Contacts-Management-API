using Contacts_Management_API.Handlers.QueryHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Contacts_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IGetContactsQueryHandler _getContactsQueryHandler;

        public ContactsController(ILogger<ContactsController> logger, IGetContactsQueryHandler getContactsQueryHandler)
        {
            _logger = logger;
            _getContactsQueryHandler = getContactsQueryHandler;
        }

        [HttpGet]
        [Route("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                var response = await _getContactsQueryHandler.GetAllContacts();

                if(response.Count == 0)
                {
                    _logger.LogInformation("No contacts are present");
                    return StatusCode(StatusCodes.Status200OK, "No contacts are present");
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetContactById/{Id}")]
        public async Task<IActionResult> GetAllContacts(int Id)
        {
            try
            {
                var response = await _getContactsQueryHandler.GetContactById(Id);

                if (response == null)
                {
                    _logger.LogInformation("No contact with given Id");
                    return StatusCode(StatusCodes.Status200OK, "No contact with given Id");
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
