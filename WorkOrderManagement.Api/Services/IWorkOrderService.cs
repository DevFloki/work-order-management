namespace WorkOrderManagement.Api.Services;

using WorkOrderManagement.Api.Models;

public interface IWorkOrderService
{
    Task<List<WorkOrder>> GetAllAsync();
    Task<WorkOrder?> GetByIdAsync(int id);
    Task<WorkOrder> CreateAsync(WorkOrder workOrder);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, WorkOrder updatedWorkOrder);

}
