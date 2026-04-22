using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        // 🔹 GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetAll()
        {
            var result = await _categoryManager.GetCategoriesAsync();
            return Ok(result);
        }

        // 🔹 GET BY ID
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<CategoryReadDTO>> GetById([FromRoute] int id)
        {
            var result = await _categoryManager.GetCategoryByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // 🔹 CREATE
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GeneralResult<CategoryReadDTO>>> CreateAsync([FromBody] CategoryCreateDTO category)
        {
            var result = await _categoryManager.CreateCategoryAsync(category);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // 🔹 UPDATE
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<GeneralResult<CategoryReadDTO>>> UpdateAsync([FromBody] CategoryEditDTO category)
        {
            var result = await _categoryManager.EditCategoryAsync(category);

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
        public async Task<ActionResult<GeneralResult<CategoryReadDTO>>> DeleteAsync([FromRoute] int id)
        {
            var result = await _categoryManager.DeleteCategoryAsync(id);

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
