using WorkOrderManagement.Api.Models;

namespace WorkOrderManagement.Api.Services
{
    public class WorkOrderService : IWorkOrderService
    {
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

        public List<WorkOrder> GetAll()
        {
            return _workOrders;
        }

        public WorkOrder? GetById(int id)
        {
            return _workOrders.FirstOrDefault(
                workOrder => workOrder.Id == id);
        }

        public WorkOrder Create(WorkOrder workOrder)
        {
            workOrder.Id = _workOrders.Any()
                ? _workOrders.Max(workOrder => workOrder.Id) + 1
                : 1;

            _workOrders.Add(workOrder);

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

        public string GetMessage()
        {
            return "Work order service is running";
        }

    }
}
