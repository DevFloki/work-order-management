using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;

namespace WorkOrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrdersController : ControllerBase
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

        [HttpGet]
        public ActionResult<List<WorkOrder>> GetAll()
        {
            return Ok(_workOrders);
        }

        [HttpGet("{id}")]
        public ActionResult<WorkOrder> GetById(int id)
        {
            WorkOrder? workOrderById = _workOrders.FirstOrDefault(
                workOrder => workOrder.Id == id);

            if (workOrderById is null)
            {
                return NotFound();
            }
            return Ok(workOrderById);
        }

        [HttpPost]
        public ActionResult<WorkOrder> Create(WorkOrder workOrder)
        {
            workOrder.Id = _workOrders.Any()
                ? _workOrders.Max(workOrder => workOrder.Id) + 1
                : 1;

            _workOrders.Add(workOrder);

            return CreatedAtAction(
                nameof(GetById),
                new { id = workOrder.Id },
                workOrder);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            WorkOrder? workOrder = _workOrders.FirstOrDefault(
                workOrder => workOrder.Id == id);

            if (workOrder is null)
            {
                return NotFound();
            }

            _workOrders.Remove(workOrder);
            return NoContent();
        }

    }
}
