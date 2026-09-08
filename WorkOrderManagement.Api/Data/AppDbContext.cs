using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Api.Models;

namespace WorkOrderManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Technician> Technicians => Set<Technician>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(customer => customer.Id)
                .HasColumnName("id");

                entity.Property(customer => customer.Name)
                .HasColumnName("name");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("assets");

                entity.Property(asset => asset.Id)
                    .HasColumnName("id");

                entity.Property(asset => asset.Name)
                    .HasColumnName("name");

                entity.Property(asset => asset.CustomerId)
                    .HasColumnName("customer_id");
            });

            modelBuilder.Entity<Technician>(entity =>
            {
                entity.ToTable("technicians");

                entity.Property(technician => technician.Id)
                    .HasColumnName("id");

                entity.Property(technician => technician.Name)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.ToTable("work_orders");

                entity.Property(workOrder => workOrder.Id)
                    .HasColumnName("id");

                entity.Property(workOrder => workOrder.Title)
                    .HasColumnName("title");

                entity.Property(workOrder => workOrder.Description)
                    .HasColumnName("description");

                entity.Property(workOrder => workOrder.Status)
                    .HasColumnName("status");

                entity.Property(workOrder => workOrder.Priority)
                    .HasColumnName("priority");

                entity.Property(workOrder => workOrder.AssetId)
                    .HasColumnName("asset_id");

                entity.Property(workOrder => workOrder.TechnicianId)
                    .HasColumnName("technician_id");
            });
        }
    }
}
