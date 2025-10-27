using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet] // Get:baseUrl/products
        public async Task<IActionResult> GetAllProducts(int? brandId, int? typeId)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(brandId,typeId);
            if (result is null) return BadRequest("result is null");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("id is null");

            var result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            if (result is null) return NotFound("id is not found");
            return Ok(result);
        }

        [HttpGet("brands")] // Get:baseUrl/products
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest("result is null");
            return Ok(result);
        }


        [HttpGet("Types")] // Get:baseUrl/products
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest("result is null");
            return Ok(result);
        }


    }
}
