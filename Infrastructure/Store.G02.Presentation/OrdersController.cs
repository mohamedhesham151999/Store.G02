using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstractions;
using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        // Create Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.orderService.CreateOrderAsync(request, userEmailClaim.Value);
            return Ok(result);
        }
    }
}
