# Work Order Management

This is a backend ASP.NET Core Web API built for maintenance work-order management.
The application provides the functionality needed to manage customers, assets, work orders
and technicians.

I am currently developing this application to learn the fundamentals of software development,
with a current focus on specializing within the C# and .NET ecosystem.

## Features

Create, view, update or delete data from the database with HTTP requests.

Query parameters to pagination, filtering and sorting desired data.

Unit & integration tests, making sure that every layer of the application works as expected.

## Tech Stack

- C#
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- xUnit
- Moq
- Microsoft.AspNetCore.Mvc.Testing
- Azure App Service
- Azure Database for postgreSQL flexible server
- Azure App Service logging

## Architecture / Project Structure

**Controllers:** Handles HTTP-requests, does input validation and calls the service class
whenever a request requires an action from the database. Limits the database information
sent back in the HTTP responses by using data transfer objects.

**Services:** Responsible for the business logic, it mainly builds and execute queries 
based on the data received from HTTP requests.

**Models:** C# classes that represents the database entities. They define the
data structure by specifying primary keys, foreign keys, 
reference navigation properties, and collection navigation properties.

**Data:** Configures the EF Core model. It bridges the C# classes to the database
by mapping entities and properties to actual tables and columns, while providing
access to the entity sets for querying.

**Tests:** Runs unit tests with Moq and integration tests with Mvc.Testing
to make sure every layer of the application is getting tested.

**Azure:** The project is also currently deployed to Microsoft Azure using a 
Resource group, App Service plan, App Service and an Azure Database for PostgreSQL flexible server.

**Diagram including Azure:** 
```
Client(HTTPS) → Azure App Service → ASP.NET Core Web API → Controller → Service
→ EF Core / DbContext → Npgsql → Azure Database for PostgreSQL
```

## API

**GetAll():** The primary GET endpoint for viewing a list of work orders with 
query parameters for status, priority, assetId, technicianId, sortBy, sortDirection, 
page and pageSize.

**GetById():** Another GET endpoint for viewing a specific work order based on its id.

**Create():** POST endpoint for creating new work orders.

**Delete():** DELETE endpoint for deleting work orders by id.

**Update():** PUT endpoint for updating work orders. 
currently have to update every column in the table.

## Running the Project

### Prerequisites

To run this Web API locally, you need to install the following tools on your machine:
* .NET 10 SDK or higher - [Download .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
* PostgreSQL Database - A running instance (local installation)
* EF Core CLI Tools - Installed via terminal `dotnet tool install --global dotnet-ef`

### Getting Started

#### 1. Configure the Database

The API uses **PostgreSQL**. Manage your user secrets in the `WorkOrderManagement.Api` project,
and remember: **User secrets contain sensitive data and should not be uploaded to github!**

Ensure your connection strings match your databases credentials:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=WorkOrderDb;Username=postgres;Password=your_password",
    "TestConnection": "Host=localhost;Database=WorkOrderTestDb;Username=postgres;Password=your_password"

  }
}
```

If the API is deployed to Microsoft Azure, then configure the App Service 
environment variable with the corresponding fields:
```
Name: ConnectionStrings__DefaultConnection
Value: Host=<server>.postgres.database.azure.com;Port=5432;Database=workorder_management;Username=<username>;Password=<password>;SSL Mode=Require;
```
App settings are initialised at runtime and can override `appsettings.json`.

#### 2. Apply Database Migrations
Navigate to the root directory where your solution (`.slnx`) or the API project sits,
and run the following command to create the database schema:

```bash
dotnet ef database update --project WorkOrderManagement.Api
```

#### 3. Run the Web API
To launch the API server, navigate to the API project directory and start it:

```bash
cd WorkOrderManagement.Api
dotnet run
```

## Testing
This solution includes an **xUnit test suite** utilizing `Moq` and `Microsoft.AspNetCore.Mvc.Testing` for integration tests.

To run all unit and integration tests, navigate to the root directory (or the test project directory) and execute:

```bash
dotnet test
```

## Deployment

The API is deployed to Microsoft Azure using:

- Azure App Service for hosting the ASP.NET Core API
- Azure Database for PostgreSQL Flexible Server for persistent storage
- App Service environment variables for production configuration and secrets
- Azure App Service diagnostics and Log Stream for application logging

The application is currently deployed manually by creating a Release
publish artifact with:

```bash
dotnet publish -c Release
```

The published output is packaged and deployed to Azure App Service.

## Notes

The application is built with four main components: `Controller → Service → EF Core → PostgreSQL`.
This is done to:
* Split up responsibilities.
* Avoid harmful actions and to not overshare data.
* Make it easier to debug, and make tests.

The application is currently just a basic CRUD API.

Only the GetAll() method currently supports filtering/sorting/pagination through 
query parameters.

The unit tests specifically tests the controller directly, isolating the methods 
so the api or database does not affect the outcome of the tests. 

Integration tests on the other hand includes several layers of the system,
so it will catch problems with the api or database.

The API is currently deployed to Microsoft Azure, but the live endpoint is not publicly linked yet because authentication/authorization has not been implemented.
