namespace WorkOrderManagement.Api.Services;

using WorkOrderManagement.Api.Models;

public interface IWorkOrderService
{
    List<WorkOrder> GetAll();
    WorkOrder? GetById(int id);
    WorkOrder Create(WorkOrder workOrder);
    bool Delete(int id);
    bool Update(int id, WorkOrder updatedWorkOrder);
    string GetMessage();

}
