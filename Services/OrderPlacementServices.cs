using OrderPlacementAPI.DBContext;
using OrderPlacementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace OrderPlacementAPI.Services
{
    public interface IOrderPlacementService
    {
        Task<OrderResponse> PlaceOrder(OrderRequest request, string idempotencyKey);
    }
    public class OrderPlacementServices: IOrderPlacementService
    {
        private readonly OrderDBContext _orderDBContext;

        public OrderPlacementServices(OrderDBContext orderDBContext)
        {
            _orderDBContext = orderDBContext ?? throw new ArgumentNullException(nameof(orderDBContext));
        }

        public async Task<OrderResponse> PlaceOrder(OrderRequest request, string idempotencyKey)
        {
            // Implement order placement logic here
            // find
            if(request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(idempotencyKey))
            {
                return new OrderResponse { Status = "Idempotency-Key header is required" };
            }
            var exisitng = await _orderDBContext.IdempotencyKeys.FindAsync(idempotencyKey);
            if(exisitng != null)
            {
                var existingOrder = await _orderDBContext.OrderDetails.FirstOrDefaultAsync(o=>o.Id == exisitng.OrderID);
                if (existingOrder == null) return new OrderResponse { Status = "Idempotency record points to missing order" };
            }
            var customer = await _orderDBContext.Customers.FindAsync(request.Customer);
            if(customer == null)
            {
                return new OrderResponse { Status = "invalid customer details" };
            }
            using var tx = await _orderDBContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in request.Items)
                {
                    if (item.quantity <= 0) return new OrderResponse { OrderId = 0, Status = "400" };

                    var product = await _orderDBContext.Products
                        .SingleOrDefaultAsync(p => p.Id == item.productId);

                    
                        if (product == null)
                        {
                            return new OrderResponse() { Status = "No product available" };
                        }
                        if (product.Stock >= item.quantity)
                        {
                            product.Stock = -item.quantity;
                        }
                        else
                        {
                            return new OrderResponse() { Status = "Insufficient product" };
                        }
                        // create the order
                        var newOrder = new OrderDetails() { ProductId = item.productId, Quantity = item.quantity, TotalAmount = item.quantity * product.Price };
                        _orderDBContext.OrderDetails.Add(newOrder);                        
                }

                await _orderDBContext.SaveChangesAsync();
                await tx.CommitAsync();
                // create the response
                var response = new OrderResponse() { OrderId = newOrder.Id, Status = "Confirmed", TotalAmount = newOrder.TotalAmount };

                return response;
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
            }
        }
    }
}
