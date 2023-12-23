using Contacts_Management_API.Controllers;
using Contacts_Management_API.Handlers.CommandHandlers;
using Contacts_Management_API.Handlers.QueryHandlers;
using Contacts_Management_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Contacts_Management_API.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private ContactsController _controller;
        private Mock<IGetContactsQueryHandler>? _mockGetContactsQueryHandler;
        private Mock<IAddContactCommandHandler>? _mockAddContactCommandHandler;
        private Mock<IUpdateContactCommandHandler>? _mockUpdateContactCommandHandler;
        private Mock<IDeleteContactCommandHandler>? _mockDeleteContactCommandHandler;
        private Mock<ILogger<ContactsController>>? _mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            _mockGetContactsQueryHandler = new Mock<IGetContactsQueryHandler>();
            _mockAddContactCommandHandler = new Mock<IAddContactCommandHandler>();
            _mockUpdateContactCommandHandler = new Mock<IUpdateContactCommandHandler>();
            _mockDeleteContactCommandHandler = new Mock<IDeleteContactCommandHandler>();
            _mockLogger = new Mock<ILogger<ContactsController>>();

            _controller = new ContactsController(
                _mockLogger.Object,
                _mockGetContactsQueryHandler.Object,
                _mockAddContactCommandHandler.Object,
                _mockUpdateContactCommandHandler.Object,
                _mockDeleteContactCommandHandler.Object
            );
        }

        [TestMethod]
        public async Task GetAllContacts_Returns_OkResult()
        {
            // Arrange
            var expectedResponse = new QueryResponseMultiple<Contact>
            {
                ErrorCode = 0,
                ErrorMessage = null,
                TotalItems = 3,
                Items = new[] {
                    new Contact { Id = 0, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" },
                    new Contact { Id = 1, FirstName = "Emily", LastName = "Johnson", Email = "emily$johnsom@outlook.edu" },
                    new Contact { Id = 2, FirstName = "Michael", LastName = "Smith", Email = "michael-smith@hotmail.org" }
                }
            };

            _mockGetContactsQueryHandler?
                .Setup(handler => handler.GetAllContacts())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllContacts();

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as QueryResponseMultiple<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.TotalItems);
        }

        [TestMethod]
        public async Task GetAllContacts_Returns_InternalServerError()
        {
            // Arrange
            var expectedResponse = new QueryResponseMultiple<Contact>
            {
                ErrorCode = 0,
                ErrorMessage = null,
                TotalItems = 3,
                Items = new[] {
                    new Contact { Id = 0, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" },
                    new Contact { Id = 1, FirstName = "Emily", LastName = "Johnson", Email = "emily$johnsom@outlook.edu" },
                    new Contact { Id = 2, FirstName = "Michael", LastName = "Smith", Email = "michael-smith@hotmail.org" }
                }
            };

            // Arrange
            _mockGetContactsQueryHandler?
                .Setup(handler => handler.GetAllContacts())
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetAllContacts();

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(500, statusResult.StatusCode);

            var response = statusResult.Value as QueryResponseMultiple<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.AreEqual("Something went wrong", response.ErrorMessage);
        }

        [TestMethod]
        public async Task GetContactById_Returns_OkResult()
        {
            // Arrange
            var expectedContactId = 1;
            var expectedContact = new Contact
            {
                Id = expectedContactId,
                FirstName = "Emily",
                LastName = "Johnson",
                Email = "emily$johnsom@outlook.edu"
            };

            var expectedResponse = new QueryResponseSingle<Contact>
            {
                ErrorCode = 0,
                ErrorMessage = null,
                Item = expectedContact
            };

            _mockGetContactsQueryHandler?
                .Setup(handler => handler.GetContactById(expectedContactId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetContactById(expectedContactId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as QueryResponseSingle<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(expectedContactId, response?.Item?.Id);
        }

        [TestMethod]
        public async Task GetContactById_Returns_BadRequest_When_IdNotProvided()
        {
            // Arrange
            var expectedResponse = new QueryResponseSingle<Contact>
            {
                ErrorCode = -1,
                ErrorMessage = "Id is required",
            };

            // Act
            var result = await _controller.GetContactById(null);

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(400, statusResult.StatusCode);

            var response = statusResult.Value as QueryResponseSingle<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.AreEqual("Id is required", response.ErrorMessage);
        }

        [TestMethod]
        public async Task GetContacts_Returns_OkResult_WithValidCriteria()
        {
            // Arrange
            var expectedResponse = new QueryResponseMultiple<Contact>
            {
                ErrorCode = 0,
                ErrorMessage = null,
                TotalItems = 3,
                Items = new[]
                {
                    new Contact { Id = 0, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" },
                    new Contact { Id = 1, FirstName = "Emily", LastName = "Johnson", Email = "emily$johnsom@outlook.edu" },
                    new Contact { Id = 2, FirstName = "Michael", LastName = "Smith", Email = "michael-smith@hotmail.org" }
                }
            };

            var query = new GetContactsQuery
            {
                FirstName = "John",
                SortField = "LastName",
                SortOrder = "asc",
                Page = 1,
                ItemsPerPage = 10
            };

            _mockGetContactsQueryHandler?
                .Setup(handler => handler.GetContacts(query))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetContacts(query);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as QueryResponseMultiple<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.TotalItems);
        }

        [TestMethod]
        public async Task GetContacts_Returns_BadRequest_WithInvalidCriteria()
        {
            // Arrange
            var expectedResponse = new QueryResponseMultiple<Contact>
            {
                ErrorCode = -1,
                ErrorMessage = "Invalid criteria provided",
            };

            var query = new GetContactsQuery
            {
                Page = -1,
                ItemsPerPage = 0
            };

            _mockGetContactsQueryHandler?
                .Setup(handler => handler.GetContacts(query))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetContacts(query);

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(200, statusResult.StatusCode);

            var response = statusResult.Value as QueryResponseMultiple<Contact>;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.AreEqual("Invalid criteria provided", response.ErrorMessage);
        }

        [TestMethod]
        public async Task AddContact_Returns_OkResult()
        {
            // Arrange
            var newContact = new Contact
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Davidson",
                Email = "alice.davidson@example.com"
            };

            var expectedResponse = new CommandResponse
            {
                ErrorCode = 0,
                ErrorMessage = null
            };

            _mockAddContactCommandHandler?
                .Setup(handler => handler.AddContact(It.IsAny<Contact>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.AddContact(newContact);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task AddContact_Returns_BadRequest_On_InvalidModel()
        {
            // Arrange
            var newContact = new Contact
            {
                Id = 3,
                FirstName = "",
                LastName = "Davidson",
                Email = "alice.davidson@example.com"
            };

            var expectedResponse = new QueryResponseSingle<Contact>
            {
                ErrorCode = -1,
                ErrorMessage = "First name is required",
            };

            _controller.ModelState.AddModelError("FirstName", "First name is required.");

            // Act
            var result = await _controller.AddContact(newContact);

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(400, statusResult.StatusCode);

            var response = statusResult.Value as CommandResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.IsNotNull(response.ErrorMessage);
        }

        [TestMethod]
        public async Task UpdateContact_Returns_OkResult()
        {
            // Arrange
            var contactToUpdate = new Contact
            {
                Id = 1,
                FirstName = "Emily",
                LastName = "Johnson_updated",
                Email = "emily$johnsom@outlook.edu_updated"
            };

            var expectedResponse = new CommandResponse
            {
                ErrorCode = 0,
                ErrorMessage = null
            };

            _mockUpdateContactCommandHandler?
                .Setup(handler => handler.UpdateContact(It.IsAny<Contact>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UpdateContact(contactToUpdate);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateContact_Returns_BadRequest_On_InvalidModel()
        {
            // Arrange
            var contactToUpdate = new Contact
            {
                Id = 1,
                FirstName = "Emily",
                LastName = "Johnson_updated",
                Email = "emily$johnsomoutlook.edu_updated"
            };

            var expectedResponse = new CommandResponse
            {
                ErrorCode = -1,
                ErrorMessage = "Invalid email format"
            };

            _controller.ModelState.AddModelError("Email", "Invalid email format");

            // Act
            var result = await _controller.UpdateContact(contactToUpdate);

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(400, statusResult.StatusCode);

            var response = statusResult.Value as CommandResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.IsNotNull(response.ErrorMessage);
        }

        [TestMethod]
        public async Task DeleteContact_Returns_OkResult()
        {
            // Arrange
            var contactIdToDelete = 2;

            var expectedResponse = new CommandResponse
            {
                ErrorCode = 0,
                ErrorMessage = null
            };

            _mockDeleteContactCommandHandler?
                .Setup(handler => handler.DeleteContact(contactIdToDelete))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteContact(contactIdToDelete);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteContact_Returns_BadRequest_When_IdNotProvided()
        {
            // Arrange
            var expectedResponse = new QueryResponseSingle<Contact>
            {
                ErrorCode = -1,
                ErrorMessage = "Id is required",
            };

            // Act
            var result = await _controller.DeleteContact(null);

            // Assert
            Assert.IsNotNull(result);
            var statusResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(400, statusResult.StatusCode);

            var response = statusResult.Value as CommandResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ErrorCode);
            Assert.AreEqual("Id is required", response.ErrorMessage);
        }
    }
}

