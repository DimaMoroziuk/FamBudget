using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {

        private ICategoryManager _categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                return new ActionResult<IEnumerable<Category>>(await _categoryManager.GetAllCategories());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCategory")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            try
            {
                return new ActionResult<Category>(await _categoryManager.GetCategoryById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory(string id, Category category)
        {
            try
            {
                await _categoryManager.UpdateExistingCategory(id, category);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("PostCategory")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                await _categoryManager.CreateNewCategory(category);
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("PostCategory", new { id = category.Id }, category);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult<Category>> DeleteCategory(IEnumerable<string> ids)
        {
            try
            {
                await _categoryManager.DeleteCategoryById(ids);
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}