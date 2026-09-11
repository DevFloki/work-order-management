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

        public async Task<List<WorkOrder>> GetAllAsync()
        {
            return await _dbContext.WorkOrders.ToListAsync();
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
