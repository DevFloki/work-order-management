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

    }
}
