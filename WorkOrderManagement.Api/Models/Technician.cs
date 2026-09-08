namespace WorkOrderManagement.Api.Models
{
    public class Technician
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<WorkOrder> WorkOrders { get; set; } = new();
    }
}
