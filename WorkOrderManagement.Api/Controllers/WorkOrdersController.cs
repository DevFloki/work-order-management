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

        private static WorkOrderResponse ToResponse(WorkOrder workOrder)
        {
            return new WorkOrderResponse
            {
                Id = workOrder.Id,
                Title = workOrder.Title,
                Description = workOrder.Description,
                Status = workOrder.Status,
                Priority = workOrder.Priority,
                AssetId = workOrder.AssetId,
                TechnicianId = workOrder.TechnicianId
            };
        }

        public async Task<string?> ValidateReferencesAsync(IWorkOrderRequest request)
        {
            if (!await _workOrderService.AssetExistsAsync(request.AssetId))
            {
                return $"Asset with id {request.AssetId} does not exist.";
            }

            if (request.TechnicianId is not null &&
                !await _workOrderService.TechnicianExistsAsync(
                    request.TechnicianId.Value))
            {
                return $"Technician with id {request.TechnicianId} does not exist.";
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkOrderResponse>>> GetAll()
        {
            List<WorkOrder> workOrders = await _workOrderService.GetAllAsync();

            List<WorkOrderResponse> response =
                workOrders.Select(ToResponse).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrderResponse>> GetById(int id)
        {
            WorkOrder? workOrderById = await _workOrderService.GetByIdAsync(id);

            if (workOrderById is null)
            {
                return NotFound();
            }

            return Ok(ToResponse(workOrderById));
        }

        [HttpPost]
        public async Task<ActionResult<WorkOrderResponse>> Create(
            CreateWorkOrderRequest request)
        {
            string? inputCheck = await ValidateReferencesAsync(request);

            if (inputCheck is not null)
            {
                return BadRequest(inputCheck);
            }

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

            WorkOrderResponse response = ToResponse(createdOrder);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _workOrderService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateWorkOrderRequest request)
        {
            string? inputCheck = await ValidateReferencesAsync(request);

            if (inputCheck is not null)
            {
                return BadRequest(inputCheck);
            }

            WorkOrder workOrder = new()
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                AssetId = request.AssetId,
                TechnicianId = request.TechnicianId
            };
            bool isUpdated = await _workOrderService.UpdateAsync(id, workOrder);

            if (!isUpdated)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
