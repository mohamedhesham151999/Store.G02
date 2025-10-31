using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstractions;
using Store.G02.Shared;
using Store.G02.Shared.Dtos;
using Store.G02.Shared.ErrorsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {


        [HttpGet] // Get:baseUrl/products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<ProductResponse>>> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(result);
        }




        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResponse>> GetProductById(int? id)
        {
            if (id is null) return BadRequest("id is null");

            var result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            //if (result is null) return NotFound("id is not found");
            return Ok(result);
        }





        [HttpGet("brands")] // Get:baseUrl/products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandTypeResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandTypeResponse>> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest("result is null");
            return Ok(result);
        }





        [HttpGet("Types")] // Get:baseUrl/products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandTypeResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandTypeResponse>> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest("result is null");
            return Ok(result);
        }


    }
}
