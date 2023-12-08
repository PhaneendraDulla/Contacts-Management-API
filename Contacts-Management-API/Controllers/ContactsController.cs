using Contacts_Management_API.Handlers.CommandHandlers;
using Contacts_Management_API.Handlers.QueryHandlers;
using Contacts_Management_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contacts_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IGetContactsQueryHandler _getContactsQueryHandler;
        private readonly IAddContactCommandHandler _addContactCommandHandler;
        private readonly IUpdateContactCommandHandler _updateContactCommandHandler;
        private readonly IDeleteContactCommandHandler _deleteContactCommandHandler;

        public ContactsController(
            ILogger<ContactsController> logger,
            IGetContactsQueryHandler getContactsQueryHandler,
            IAddContactCommandHandler addContactCommandHandler,
            IUpdateContactCommandHandler updateContactCommandHandler,
            IDeleteContactCommandHandler deleteContactCommandHandler)
        {
            _logger = logger;
            _getContactsQueryHandler = getContactsQueryHandler;
            _addContactCommandHandler = addContactCommandHandler;
            _updateContactCommandHandler = updateContactCommandHandler;
            _deleteContactCommandHandler = deleteContactCommandHandler;
        }

        [HttpGet]
        [Route("GetAllContacts")]
        public async Task<ActionResult<IResponse>> GetAllContacts()
        {
            try
            {
                var response = await _getContactsQueryHandler.GetAllContacts();

                if (response.ErrorCode == -1)
                {
                    _logger.LogInformation(response.ErrorMessage);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                var response = new QueryResponseMultiple<Contact>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = -1
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("GetContactById")]
        public async Task<ActionResult<IResponse>> GetContactById([FromQuery] int Id)
        {
            try
            {
                var response = await _getContactsQueryHandler.GetContactById(Id);

                if (response.ErrorCode == -1)
                {
                    _logger.LogInformation(response.ErrorMessage);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                var response = new QueryResponseSingle<Contact>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = -1
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("AddContact")]
        public async Task<ActionResult<IResponse>> AddContact([FromBody] Contact newContact)
        {
            try
            {
                var response = await _addContactCommandHandler.AddContact(newContact);

                if (response.ErrorCode == -1)
                {
                    _logger.LogInformation(response.ErrorMessage);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                var response = new QueryResponseSingle<Contact>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = -1
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("UpdateContact")]
        public async Task<ActionResult<IResponse>> UpdateContact([FromBody] Contact contact)
        {
            try
            {
                var response = await _updateContactCommandHandler.UpdateContact(contact);

                if (response.ErrorCode == -1)
                {
                    _logger.LogInformation(response.ErrorMessage);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                var response = new QueryResponseSingle<Contact>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = -1
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        [Route("DeleteContact")]
        public async Task<ActionResult<IResponse>> DeleteContact([FromQuery] int Id)
        {
            try
            {
                var response = await _deleteContactCommandHandler.DeleteContact(Id);

                if (response.ErrorCode == -1)
                {
                    _logger.LogInformation(response.ErrorMessage);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                var response = new QueryResponseSingle<Contact>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = -1
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
