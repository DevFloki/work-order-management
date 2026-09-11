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

        private static readonly List<WorkOrder> _workOrders = new()
        {
            new WorkOrder
            {
                Id = 1,
                Title = "Repair conveyor belt",
                Description = "Conveyor belt has stopped moving.",
                Status = "Open",
                Priority = "High"
            },

            new WorkOrder
            {
                Id = 2,
                Title = "Inspect ventilation system",
                Description = "Routine inspection",
                Status = "Open",
                Priority = "Medium"

            }
        };

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

        public bool Delete(int id)
        {
            WorkOrder? workOrder = _workOrders.FirstOrDefault(
                workOrder => workOrder.Id == id);

            if (workOrder is null)
            {
                return false;
            }

            _workOrders.Remove(workOrder);
            return true;
        }

        public bool Update(int id, WorkOrder updatedWorkOrder)
        {
            WorkOrder? workOrderById = _workOrders.FirstOrDefault(
                workOrder => workOrder.Id == id);

            if (workOrderById is null)
            {
                return false;
            }

            workOrderById.Title = updatedWorkOrder.Title;
            workOrderById.Description = updatedWorkOrder.Description;
            workOrderById.Status = updatedWorkOrder.Status;
            workOrderById.Priority = updatedWorkOrder.Priority;
            return true;
        }

    }
}
