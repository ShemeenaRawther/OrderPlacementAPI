using System.ComponentModel.DataAnnotations;

namespace OrderPlacementAPI.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderDetails
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }        
    }

    public class IdempotencyKey
    {
        [Key]
        public string Key { get; set; } = null;
        public int OrderID { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
