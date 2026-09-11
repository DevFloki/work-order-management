using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagement.Api.Models
{
    public class CreateWorkOrderRequest : IWorkOrderRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = "";
        [StringLength(1000)]
        public string Description { get; set; } = "";
        [Required]
        [AllowedValues("Open", "InProgress", "Completed")]
        public string Status { get; set; } = "";
        [Required]
        [AllowedValues("Low", "Medium", "High")]
        public string Priority { get; set; } = "";
        [Range(1, int.MaxValue)]
        public int AssetId { get; set; }
        [Range(1, int.MaxValue)]
        public int? TechnicianId { get; set; }
    }
}
