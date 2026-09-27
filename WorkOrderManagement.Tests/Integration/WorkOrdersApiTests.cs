using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using WorkOrderManagement.Api.Data;
using WorkOrderManagement.Api.Models;


namespace WorkOrderManagement.Tests.Integration
{
    public class WorkOrdersApiTests
        : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public WorkOrdersApiTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;

            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("https://localhost")
                });
        }

        public async ValueTask InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync(
                TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var cancellationToken =
                TestContext.Current.CancellationToken;

            await db.WorkOrders.ExecuteDeleteAsync(cancellationToken);
            await db.Assets.ExecuteDeleteAsync(cancellationToken);
            await db.Technicians.ExecuteDeleteAsync(cancellationToken);
            await db.Customers.ExecuteDeleteAsync(cancellationToken);
        }

        [Fact]
        public async Task GetAll_WhenPageIsZero_ReturnBadRequest()
        {
            var response = await _client.GetAsync(
                "/api/workorders?page=0",
                TestContext.Current.CancellationToken);

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }

        [Fact]
        public async Task GetById_WhenWorkOrderExists_ReturnsWorkOrderFromDatabase()
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            int workOrderId;
            const string expectedTitle = "Integration test - Fix pump";

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var customer = new Customer
                {
                    Name = "Integration Test Customer"
                };

                var asset = new Asset
                {
                    Name = "Integration Test Pump",
                    Customer = customer
                };

                var workOrder = new WorkOrder
                {
                    Title = expectedTitle,
                    Description = "Pump is leaking",
                    Status = "Open",
                    Priority = "High",
                    Asset = asset
                };

                db.WorkOrders.Add(workOrder);

                await db.SaveChangesAsync(cancellationToken);

                workOrderId = workOrder.Id;
            }

            var response = await _client.GetAsync(
                $"/api/workorders/{workOrderId}",
                cancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var model = await response.Content
                .ReadFromJsonAsync<WorkOrderResponse>(
                    cancellationToken);

            Assert.NotNull(model);
            Assert.Equal(workOrderId, model.Id);
            Assert.Equal(expectedTitle, model.Title);
        }

        [Fact]
        public async Task Create_WithValidRequest_PersistsWorkOrderAndReturnsCreated()
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            int assetId;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var customer = new Customer
                {
                    Name = "Integration Test Customer"
                };

                var asset = new Asset
                {
                    Name = "Integration Test Pump",
                    Customer = customer
                };

                db.Assets.Add(asset);

                await db.SaveChangesAsync(cancellationToken);

                assetId = asset.Id;
            }

            var request = new CreateWorkOrderRequest
            {
                Title = "Replace bearing",
                Description = "Bearing is worn",
                Status = "Open",
                Priority = "High",
                AssetId = assetId
            };

            var response = await _client.PostAsJsonAsync(
                "/api/workorders",
                request,
                cancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var model = await response.Content
                .ReadFromJsonAsync<WorkOrderResponse>(
                    cancellationToken);

            Assert.NotNull(model);
            Assert.Equal("Replace bearing", model.Title);
            Assert.Equal(assetId, model.AssetId);

            using var verifyScope = _factory.Services.CreateScope();

            var verifyDb = verifyScope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var savedWorkOrder = await verifyDb.WorkOrders
                .SingleOrDefaultAsync(
                    w => w.Id == model.Id,
                    cancellationToken);

            Assert.NotNull(savedWorkOrder);
            Assert.Equal("Replace bearing", savedWorkOrder.Title);
        }

    }
}