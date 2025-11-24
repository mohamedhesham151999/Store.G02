using Microsoft.AspNetCore.Authorization;
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

        // get all delivery methods
        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await _serviceManager.orderService.GetAllDeliveryMethodAsync();
            return Ok(result);
        }

        // get all ord  er
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.orderService.GetOrdersForSpecificUserAsync(userEmailClaim.Value);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdForSpecificUserAsync(Guid id)
         {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);

            var result = await _serviceManager.orderService.GetOrderByIdForSpecificUserAsync(id, userEmailClaim.Value);
            return Ok(result);
        }

    }
}
