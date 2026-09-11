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
        public async Task<ActionResult<List<WorkOrder>>> GetAll()
        {
            List<WorkOrder> workOrders = await _workOrderService.GetAllAsync();

            return Ok(workOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrder>> GetById(int id)
        {
            WorkOrder? workOrderById = await _workOrderService.GetByIdAsync(id);

            if (workOrderById is null)
            {
                return NotFound();
            }

            return Ok(workOrderById);
        }

        [HttpPost]
        public async Task<ActionResult<WorkOrder>> Create(
            CreatedWorkOrderRequest request)
        {
            WorkOrder workOrder = new WorkOrder
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                AssetId = request.AssetId,
                TechnicianId = request.TechnicianId
            };

            WorkOrder createdOrder =
                await _workOrderService.CreateAsync(workOrder);

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
