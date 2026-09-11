namespace WorkOrderManagement.Api.Models
{
    public class WorkOrderResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public string Priority { get; set; } = "";
        public int AssetId { get; set; }
        public int? TechnicianId { get; set; }
    }
}
