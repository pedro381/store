# Store

![License](https://img.shields.io/badge/license-MIT-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-brightgreen)

**Store** is a modern, scalable .NET application designed for managing orders and various store operations. Built with Clean Architecture principles and Domain-Driven Design (DDD), the solution is organized into several layers that facilitate separation of concerns, ease of maintenance, and scalability.

## Project Structure

The repository is organized into multiple projects:

- **Store.Api**:  
  This is the web API layer exposing RESTful endpoints, handling startup configurations, and integrating middleware components for exception handling, logging, health checks, and Swagger documentation.  
  - *Note*: The `Store.Api.csproj` targets **net8.0** and uses the `Microsoft.NET.Sdk.Web` SDK. It includes references such as the Entity Framework Core Design package and links to the CrossCutting project.

- **Store.Application**:  
  Contains the business logic, including command and query handlers, and mapping profiles. This layer orchestrates the operations required to process requests, ensuring that business rules are followed.

- **Store.CrossCutting**:  
  Provides common services and utilities used throughout the application. This includes:
  - **Authentication**: Implements token services for secure access.
  - **Extensions**: Helpers for dependency injection, middleware registration (e.g., body logging, correlation ID management, exception handling), health checks, and Swagger configuration.

- **Store.Domain**:  
  Represents the core business entities and domain logic. This layer defines the fundamental objects (entities, DTOs, base classes) and encapsulates the business rules and validations.

- **Store.Infrastructure**:  
  Manages data access and persistence:
  - Implements the Entity Framework Core DbContext.
  - Contains repositories (e.g., OrderRepository, DiscountConfigurationRepository) for accessing the database.
  - Handles database migrations to keep the schema up-to-date.

- **Store.Repository & Store.Service**:  
  These projects contain more specific implementations related to repositories and domain services.

- **Tests**:  
  The solution includes automated tests to ensure the quality and integrity of the codebase. Tests are organized into a dedicated project that covers unit and integration scenarios.

## Key Features

- **Order Management**:  
  - Create, update, and delete orders along with detailed management of order items.
  - Query functionality to fetch orders by number or retrieve a list of orders.

- **Authentication & Authorization**:  
  - Secure endpoints using token-based authentication.
  - Enforces user roles and permissions across the API.

- **Health Checks and Monitoring**:  
  - Integrates health checks to monitor the status of the database and critical services.
  - Provides middleware to log request/response details and correlation IDs for tracking.

- **Clean Architecture and Domain-Driven Design**:  
  - The project structure promotes separation of concerns by dividing the solution into distinct layers (API, Application, Domain, Infrastructure, etc.).
  - Encourages testability and maintainability through clear boundaries and well-defined interfaces.

## Technologies Used

- **.NET 8.0**:  
  The project leverages the latest .NET version for improved performance, language features, and long-term support.

- **Entity Framework Core**:  
  Used for database interactions and migrations. The `Microsoft.EntityFrameworkCore.Design` package is referenced (as seen in the Store.Api csproj) to support design-time services.

- **Clean Architecture / Domain-Driven Design**:  
  These principles are employed to ensure a modular, scalable, and maintainable codebase.

- **Swagger**:  
  Integrated for API documentation, making it easier for developers and stakeholders to understand and test endpoints.

- **Testing Frameworks**:  
  Automated tests are implemented (using frameworks such as XUnit or similar) to cover unit and integration scenarios, ensuring robustness and reliability.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A SQL database (e.g., SQL Server, PostgreSQL) configured as per project requirements.
- NuGet package manager for dependency restoration.

## Installation and Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/pedro381/store.git
   cd store
   ```

2. **Restore Dependencies:**

   ```bash
   dotnet restore
   ```

3. **Configure the Database:**

   Update the connection strings in the configuration files (e.g., `appsettings.json` in the API or Infrastructure project) to point to your database instance.

4. **Apply Database Migrations (if applicable):**

   ```bash
   dotnet ef database update --project Store.Infrastructure
   ```

5. **Run the Application:**

   Navigate to the API project directory and run:

   ```bash
   dotnet run --project Store.Api
   ```

## Running the Tests

Execute the test suite using the following command from the root directory:

```bash
dotnet test
```

## Contributing

Contributions are welcome! Follow these steps to contribute:

1. Fork the repository.
2. Create a new branch for your feature:  
   `git checkout -b my-feature`
3. Commit your changes:  
   `git commit -m "Add new feature"`
4. Push to your branch:  
   `git push origin my-feature`
5. Open a Pull Request.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

Pedro – [GitHub](https://github.com/pedro381)