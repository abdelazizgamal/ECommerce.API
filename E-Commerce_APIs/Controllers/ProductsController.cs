using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductManager _productManager;
        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResult<PagedResult<ProductReadDTO>>>> GetAllAsync
            (
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] int? categoryId = null,
                [FromQuery] string? name = null
            )
        {
            var paginationParameters = new PaginationParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var productFilterParameters = new ProductFilterParameters
            {
                CategoryId = categoryId,
                Search = name
            };

            var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<ActionResult<GeneralResult<ProductReadDTO>>> GetById([FromRoute] int id)
        {
            var state = await _productManager.GetProductByIdAsync(id);
            if (!state.Success)
            {
                return NotFound(state);
            }
            return Ok(state);
        }


        [HttpPost]
        [Route("")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult<ProductReadDTO>>> CreateAsync([FromBody] ProductCreateDTO product)
        {
            var result = await _productManager.CreateProductAsync(product);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        // 🔹 UPDATE
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult<ProductReadDTO>>> UpdateAsync([FromRoute] int id, [FromBody] ProductEditDTO product)
        {
            product.Id = id;
            var result = await _productManager.EditProductAsync(product);

            if (!result.Success)
            {
                if (result.Message == "Resource not found")
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }


        // 🔹 DELETE
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult<ProductReadDTO>>> DeleteAsync([FromRoute] int id)
        {
            var result = await _productManager.DeleteProduct(id);

            if (!result.Success)
            {
                if (result.Message == "Resource not found")
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
