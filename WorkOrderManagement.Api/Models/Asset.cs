namespace WorkOrderManagement.Api.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<WorkOrder> WorkOrders { get; set; } = new();
    }
}
