using AutoMapper;
using CRUD.Data;
using CRUD.Dtos;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {   
        private readonly IMapper _mapper;
        private readonly ICategoriesService _categoriesService;
      
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoriesService.GetCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoriesService.GetCategoryAsync(id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm]CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = await _categoriesService.ExistingCategory(dto.Name);

            if (existingCategory != null)
                return BadRequest($"A Category with the name {dto.Name} already exists.");
            
            var category = _mapper.Map<Category>(dto);

            _categoriesService.CreateCategoryAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id,[FromForm] CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoriesService.GetCategoryAsync(id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            var existingCategory = await _categoriesService.ExistingCategory(dto.Name);
            if (existingCategory != null)
                return BadRequest($"A Category with the name {dto.Name} already exists.");

            _mapper.Map(category, dto);

            _categoriesService.UpdateCategoryAsync(category);

            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoriesService.GetCategoryAsync(id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            
            var hasBooks = await _categoriesService.HasBooks(id);
            if (hasBooks)
                return BadRequest("Cannot delete a category that has associated books.");

            _categoriesService.DeleteCategoryAsync(category);
            return Ok($"Category with ID: {id} was deleted successfully.");
        }
    }
}
