using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderPlacementAPI.Model;
using OrderPlacementAPI.Services;

namespace OrderPlacementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPlacementController : ControllerBase
    {
        private readonly IOrderPlacementService _orderPlacementService;

        public OrderPlacementController(IOrderPlacementService orderPlacementService)
        {
            _orderPlacementService = orderPlacementService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrders([FromBody] OrderRequest request)
        {
            var idempotencyKey= Request.Headers["Idempotency-Key"].FirstOrDefault();
            if(request is null || request.Customer <=0)
            {
                return BadRequest("invalid request");
            }

            var response = await _orderPlacementService.PlaceOrder(request, idempotencyKey);
            return Ok(response);
        }
    }
}
