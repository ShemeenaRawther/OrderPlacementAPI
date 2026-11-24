namespace OrderPlacementAPI.Model
{
    public class OrderRequest
    {
        public int Customer { get; set; }
        public List<OrderRequestItem> Items { get; set; } = new();    
    }
    public class OrderRequestItem
    {
        public int productId { get; set; }
        public int quantity { get; set; }
    }

    public class OrderResponse
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public double TotalAmount { get; set; }        
    }
}
