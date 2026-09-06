using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Api.Services;

namespace WorkOrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }


        [HttpGet]
        public ActionResult<List<WorkOrder>> GetAll()
        {
            return Ok(_workOrderService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<WorkOrder> GetById(int id)
        {
            WorkOrder? workOrderById = _workOrderService.GetById(id);

            if (workOrderById is null)
            {
                return NotFound();
            }

            return Ok(workOrderById);
        }

        [HttpPost]
        public ActionResult<WorkOrder> Create(WorkOrder workOrder)
        {
            WorkOrder createdOrder = _workOrderService.Create(workOrder);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdOrder.Id },
                createdOrder);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _workOrderService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, WorkOrder updatedWorkOrder)
        {
            bool isUpdated = _workOrderService.Update(id, updatedWorkOrder);

            if (!isUpdated)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
