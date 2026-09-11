using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagement.Api.Models
{
    public interface IWorkOrderRequest
    {
        public int AssetId { get; set; }
        [Range(1, int.MaxValue)]
        public int? TechnicianId { get; set; }
    }
}
