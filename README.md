# Contacts-Management-API
This Web API manages the contact Information



## Setup Instructions:

### Prerequisites:
- .NET Core 8 SDK
- IDE (e.g., Visual Studio, Visual Studio Code)

### Steps:
1. Clone the project repository.
2. Open the project in your preferred IDE, I used Visual Studio 2022.
3. Configure environment variables in the launch settings file.
4. Restore NuGet packages and build the solution.
5. Run the application using the IDE's run/debug option.



## Application Run:

### Profiles: 
- **http**: Runs on `http://localhost:5153` with Swagger UI.
- **https**: Runs on `https://localhost:7092` and `http://localhost:5153` with Swagger UI.
- **IIS Express**: Deploys via IIS Express with Swagger UI. It runs on  `http://localhost:44367`

I have used `IIS Express Profile` and its respective port in UI Project.

Select `IIS Express profile` in your IDE and run the application. Access Swagger UI via the provided URLs to interact with API endpoints.

Make sure you're running this API (`http://localhost:44367`), before you run the Angular UI project.



## Design Decisions & Application Structure:

### Controllers and Handlers:
- `ContactsController` manages CRUD (Create, Read, Update, Delete) operations for contacts. It serves as an entry point for incoming HTTP requests.
- Handlers maintain command and query logic for separation of concerns. For instance, `GetContactsQueryHandler`, `AddContactCommandHandler`, `UpdateContactCommandHandler`, and `DeleteContactCommandHandler` execute specific operations, ensuring a clear division of responsibilities.

### Models and Responses:
- `Contact` model defines the structure of contact information, including properties like ID, first name, last name, and email. This structuring helps maintain consistency and organization in data handling.
- Response models (`QueryResponseSingle`, `QueryResponseMultiple`, `CommandResponse`) standardize API responses. This uniformity ensures consistent and predictable communication between the API and clients, making error handling and data retrieval more manageable.

### Swagger/OpenAPI:
- Integration of Swagger/OpenAPI provides comprehensive documentation for the API endpoints. 
- It offers an interactive UI that enables developers to explore, understand, and test the API functionalities effortlessly. This documentation is vital for facilitating API usage and encouraging adoption among developers.

### Error Handling:
- Exceptions are logged and returned as standardized error responses for improved API reliability.
- These responses include error codes, error messages, and relevant status codes. This consistent error-handling mechanism enhances the reliability and robustness of the API, allowing clients to handle errors more efficiently.

### Environmental Profiles:
- Leveraging different profiles in launch settings (such as http, https, IIS Express) facilitates debugging and testing across various environments. Each profile offers a distinct setup, enabling developers to seamlessly switch between configurations for development, testing, and deployment purposes.



## Application Flow:

## Endpoints and Operations:

### 1. Get All Contacts:
- **Endpoint:** `GET /api/Contacts/GetAllContacts`
- Fetches all contacts from the database.
- Returns a list of contacts.

### 2. Get Contact by ID:
- **Endpoint:** `GET /api/Contacts/GetContactById?id={contactId}`
- Retrieves a contact by its unique identifier.
- Returns a single contact entity.

### 3. Add a Contact:
- **Endpoint:** `POST /api/Contacts/AddContact`
- Creates a new contact in the database.
- Expects contact details in the request body.
- Returns success message or error response.

### 4. Update a Contact:
- **Endpoint:** `PUT /api/Contacts/UpdateContact`
- Modifies an existing contact in the database.
- Expects updated contact details in the request body.
- Returns success message or error response.

### 5. Delete a Contact:
- **Endpoint:** `DELETE /api/Contacts/DeleteContact?id={contactId}`
- Removes a contact from the database by its ID.
- Returns success message or error response.



## Test Coverage:

### Unit Tests:

#### ContactsControllerTests:

- **Covered CRUD operations in ContactsController:**
  - **Get all contacts:** Tested the retrieval of all contacts.
  - **Get contact by ID:** Verified the retrieval of a contact based on its unique identifier.
  - **Add a new contact:** Tested the creation of a new contact.
  - **Update an existing contact:** Verified the modification of an existing contact.
  - **Delete a contact:** Tested the removal of a contact.



## Execution Flow:

1. **Initialization:**
   - Application starts, initializing required services and dependencies.

2. **HTTP Request Handling:**
   - Incoming HTTP requests are directed to respective controller methods based on route mappings.

3. **Controller Handling:**
   - Controllers receive requests and invoke the appropriate handler methods.

4. **Handler Execution:**
   - Command and Query handlers process incoming requests based on their responsibilities.
   - Command handlers execute write operations (add, update, delete).
   - Query handlers execute read operations (get, retrieve).

5. **Data Interaction:**
   - Handlers interact with the JSON Mock Data.
   - Fetch, modify, or delete records based on the requested operation.

6. **Response Handling:**
   - Handlers generate appropriate response models (success or error) based on database operations.
   - Responses are returned to the controller.

7. **HTTP Response:**
   - Controllers send HTTP responses back to the client with appropriate status codes and response bodies.

8. **Logging and Error Handling:**
   - Exception handling and logging capture any errors during execution, returning standardized error responses.

9. **API Interaction:**
   - Clients interact with the API endpoints using HTTP requests (GET, POST, PUT, DELETE).
