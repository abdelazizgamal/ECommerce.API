using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductManager _productManager;
        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [Route("Pagination")]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDTO>>>> GetAllPaginationAsync
            (
                [FromQuery] PaginationParameters paginationParameters
            )
        {
            var result = await _productManager.GetProductsPaginationAsync(paginationParameters);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDTO>>>> GetAll()
        {
            var result = await _productManager.GetProductsAsync();
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
        [Route("Create")]
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
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<GeneralResult<ProductReadDTO>>> UpdateAsync([FromBody] ProductEditDTO product)
        {
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
        [HttpDelete]
        [Route("Delete/{id:int}")]
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
