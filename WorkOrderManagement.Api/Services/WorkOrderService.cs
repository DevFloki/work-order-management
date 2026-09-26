using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Api.Data;
using WorkOrderManagement.Api.Models;

namespace WorkOrderManagement.Api.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly AppDbContext _dbContext;

        public WorkOrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<WorkOrder>> GetAllAsync(
            string? status,
            string? priority,
            int? assetId,
            int? technicianId,
            string? sortBy,
            string? sortDirection,
            int? page,
            int? pageSize)
        {
            IQueryable<WorkOrder> workOrders = _dbContext.WorkOrders;

            if (!string.IsNullOrWhiteSpace(status))
                workOrders = workOrders.Where(w => w.Status == status);

            if (!string.IsNullOrWhiteSpace(priority))
                workOrders = workOrders.Where(w => w.Priority == priority);

            if (assetId is not null)
                workOrders = workOrders.Where(w => w.AssetId == assetId);

            if (technicianId is not null)
                workOrders = workOrders.Where(w => w.TechnicianId == technicianId);

            bool isDesc = string.Equals(
                sortDirection,
                "desc",
                StringComparison.OrdinalIgnoreCase);

            workOrders = sortBy?.ToLower() switch
            {
                "title" => isDesc
                ? workOrders
                .OrderByDescending(w => w.Title)
                .ThenByDescending(w => w.Id)
                : workOrders
                .OrderBy(w => w.Title)
                .ThenBy(w => w.Id),

                _ => isDesc
                ? workOrders.OrderByDescending(w => w.Id)
                : workOrders.OrderBy(w => w.Id)
            };

            int actualSize = pageSize ?? 10;
            int actualPage = page ?? 1;
            int skip = (actualPage - 1) * actualSize;
            workOrders = workOrders.Skip(skip).Take(actualSize);

            return await workOrders.ToListAsync();
        }

        public async Task<WorkOrder?> GetByIdAsync(int id)
        {
            return await _dbContext.WorkOrders.FirstOrDefaultAsync(
                workOrder => workOrder.Id == id);
        }

        public async Task<WorkOrder> CreateAsync(WorkOrder workOrder)
        {
            _dbContext.Add(workOrder);

            await _dbContext.SaveChangesAsync();

            return workOrder;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            WorkOrder? workOrder = await _dbContext.WorkOrders.FirstOrDefaultAsync(
                workOrder => workOrder.Id == id);

            if (workOrder is null)
            {
                return false;
            }

            _dbContext.Remove(workOrder);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, WorkOrder updatedWorkOrder)
        {
            WorkOrder? existingWorkOrder = await _dbContext.WorkOrders.FirstOrDefaultAsync(
                workOrder => workOrder.Id == id);

            if (existingWorkOrder is null)
            {
                return false;
            }

            existingWorkOrder.Title = updatedWorkOrder.Title;
            existingWorkOrder.Description = updatedWorkOrder.Description;
            existingWorkOrder.Status = updatedWorkOrder.Status;
            existingWorkOrder.Priority = updatedWorkOrder.Priority;
            existingWorkOrder.AssetId = updatedWorkOrder.AssetId;
            existingWorkOrder.TechnicianId = updatedWorkOrder.TechnicianId;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssetExistsAsync(int id)
        {
            return await _dbContext.Assets.AnyAsync(
                asset => asset.Id == id);
        }

        public async Task<bool> TechnicianExistsAsync(int id)
        {
            return await _dbContext.Technicians.AnyAsync(
                technician => technician.Id == id);
        }

    }
}
