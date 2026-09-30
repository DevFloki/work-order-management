using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkOrderManagement.Api.Data;

namespace WorkOrderManagement.Tests.Integration;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices((context, services) =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<AppDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            string testConnection =
                context.Configuration.GetConnectionString("TestConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'TestConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(testConnection));
        });
    }
}
