namespace WorkOrderManagement.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<Asset> Assets { get; set; } = new();
    }
}
