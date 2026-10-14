# Work Order Management
An ASP.NET Core Web API for managing maintenance work orders associated with assets and technicians.
The project focuses on REST API design, database access, automated testing, and deployment to Microsoft Azure.

The application is under active development.

## Features
- **Work order management:** Create, retrieve, update, and delete work orders.
- **Filtering and pagination:** Filter work orders by status, priority, asset, or technician; sort by ID or title; and paginate results.
- **Input validation:** Validate request fields and check that referenced assets and technicians exist.
- **Data transfer objects:** Separate API request/response models from database entities.
- **Automated testing:** Unit tests for controllers and integration tests that exercise the API against PostgreSQL.
- **CI/CD:** GitHub Actions builds, tests, and publishes the application, with deployment to Azure App Service on pushes to `main`.
- **Health checks:** A `/health` endpoint for basic application health reporting.

## Tech Stack
| Area | Technologies |
| --- | --- |
| Language and framework | C#, .NET 10, ASP.NET Core Web API |
| Data access | Entity Framework Core, Npgsql |
| Database | PostgreSQL |
| Testing | xUnit, Moq, `Microsoft.AspNetCore.Mvc.Testing` |
| CI/CD | GitHub Actions |
| Hosting and diagnostics | Azure App Service, Azure Database for PostgreSQL, Azure App Service logging |

## Architecture

The application separates HTTP request handling, business logic, and data access:

```text
HTTP client
    ↓
WorkOrdersController (HTTP endpoints, validation, response mapping)
    ↓
IWorkOrderService / WorkOrderService (work-order operations and queries)
    ↓
AppDbContext / Entity Framework Core
    ↓
PostgreSQL
```

- **Controllers** accept and validate requests, call the service layer, and return HTTP responses.
- **Services** implement work-order operations, including filters, sorting, and pagination.
- **Models and DTOs** define persisted entities and the data exposed by the API.
- **Data and migrations** configure PostgreSQL persistence through Entity Framework Core.
- **Tests** verify controller behavior and the API's interaction with a test database.

## API endpoints
| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/workorders` | List work orders, with optional filters, sorting, and pagination |
| `GET` | `/api/workorders/{id}` | Retrieve one work order |
| `POST` | `/api/workorders` | Create a work order |
| `PUT` | `/api/workorders/{id}` | Replace an existing work order's editable fields |
| `DELETE` | `/api/workorders/{id}` | Delete a work order |
| `GET` | `/health` | Basic application health check |

### Querying work orders
Example:

```http
GET /api/workorders?status=Open&priority=High&sortBy=title&sortDirection=asc&page=1&pageSize=10
```

Supported filters are `status`, `priority`, `assetId`, and `technicianId`. Sorting uses `sortBy` (`title` or the default `id` order) and `sortDirection` (`asc` or `desc`). Pagination defaults to page 1 with 10 results per page; `pageSize` is limited to 100.

Supported statuses are `Open`, `InProgress`, and `Completed`. Supported priorities are `Low`, `Medium`, and `High`.

### Creating a work order
Example request:

```http
@WorkOrderManagement.Api_HostAddress = http://localhost:5094

POST {{WorkOrderManagement.Api_HostAddress}}/api/workorders
Content-Type: application/json
{
  "title": "Test title",
  "description": "Test description",
  "status": "Open",
  "priority": "Medium",
  "assetId": 1,
  "technicianId": null
}
```

`assetId` must reference an existing asset. If provided, `technicianId` must reference an existing technician. A successful creation returns `201 Created`; invalid input or references return `400 Bad Request`.

The API currently provides endpoints for **work orders only**; it does not expose separate endpoints to create assets, customers, or technicians.

## Running locally

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- A running PostgreSQL instance
- EF Core command-line tools: `dotnet tool install --global dotnet-ef`

### 1. Clone the repository
```bash
git clone https://github.com/DevFloki/work-order-management.git
cd work-order-management
```

### 2. Configure PostgreSQL
Create a development database, such as `WorkOrderDb`. For integration testing, create a separate database, such as `WorkOrderTestDb`.

Store connection strings using .NET user secrets rather than committing credentials:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=WorkOrderDb;Username=postgres;Password=YOUR_PASSWORD" --project WorkOrderManagement.Api

dotnet user-secrets set "ConnectionStrings:TestConnection" "Host=localhost;Database=WorkOrderTestDb;Username=postgres;Password=YOUR_PASSWORD" --project WorkOrderManagement.Api
```

Replace `YOUR_PASSWORD` and database names as needed. The application uses `DefaultConnection`; the integration tests use `TestConnection`.

### 3. Apply migrations
From the repository root:

```bash
dotnet ef database update --project WorkOrderManagement.Api
```

### 4. Run the API
```bash
dotnet run --project WorkOrderManagement.Api --launch-profile http
```

With the repository's HTTP launch profile, the API runs at `http://localhost:5094`.

Try the health check:

```bash
curl http://localhost:5094/health
```

The application exposes OpenAPI documentation in the **Development** environment at `/openapi/v1.json`. This is an OpenAPI JSON document, not a Swagger UI.

## Testing
The test project contains controller unit tests and integration tests using `WebApplicationFactory` and a separate PostgreSQL database.

Run all tests from the repository root:

```bash
dotnet test
```

Integration tests apply EF Core migrations to the configured test database. Use a dedicated test database; tests modify and clean up test data.

## CI/CD and Azure deployment
The GitHub Actions workflow in `.github/workflows/ci.yml` runs on pushes and pull requests. It restores dependencies, builds the solution, runs tests with a PostgreSQL service container, and publishes the API artifact.

On pushes to `main`, the workflow also authenticates to Azure through OpenID Connect (OIDC) and deploys the published application to **Azure App Service**.

The cloud setup uses Azure Database for PostgreSQL and environment-based connection-string configuration. Application diagnostics are supported through Azure App Service logging.

The deployed API is **not published here as an open public endpoint** because authentication and authorization have not yet been implemented.

## Current scope and limitations
- The implemented public API surface manages **work orders**; customer, asset, and technician management endpoints are not currently exposed.
- Filtering, sorting, and pagination are available on the work-order list endpoint.
- Updates use `PUT` and supply the complete set of editable work-order fields; partial updates are not implemented.
- Authentication and authorization have not yet been implemented. The hosted API is not intended for unrestricted public use.
- The `/health` endpoint provides a basic application health check; it is not a database connectivity test.

## License

This project is licensed under the [MIT License](LICENSE).
