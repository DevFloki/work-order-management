namespace WorkOrderManagement.Api.Services;

using WorkOrderManagement.Api.Models;

public interface IWorkOrderService
{
    Task<List<WorkOrder>> GetAllAsync();
    Task<WorkOrder?> GetByIdAsync(int id);
    Task<WorkOrder> CreateAsync(WorkOrder workOrder);
    bool Delete(int id);
    bool Update(int id, WorkOrder updatedWorkOrder);

}
