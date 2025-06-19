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
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorsService _authorsService;
        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorsService.GetAuthorsAsync();

            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _authorsService.GetAuthorAsync(id);

            if (author == null)
            {
                return NotFound($"No Author Was Found With ID: {id}");
            }
            return Ok(author);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromForm] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAuthor = await _authorsService.ExistingAuthor(dto.Email);

            if (existingAuthor != null)
                return BadRequest($"An Author with the email {dto.Email} already exists.");
            
            var author = _mapper.Map<Author>(dto);

            _authorsService.CreateAuthorAsync(author);
           

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromForm]AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _authorsService.GetAuthorAsync(id);
            if (author == null)
                return NotFound($"No Author Was Found With ID: {id}");

            var existingAuthor = await _authorsService.ExistingAuthor(dto.Email);
                
            if (existingAuthor != null)
                return BadRequest($"An Author with the email {dto.Email} already exists.");

            _mapper.Map(dto, author);

            _authorsService.UpdateAuthorAsync(author);
            return Ok(author);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorsService.GetAuthorAsync(id);
            if (author == null)
                return NotFound($"No Author Was Found With ID: {id}");
            
            var hasBooks = await _authorsService.HasBooks(id);
            if (hasBooks)
                return BadRequest("Cannot delete an author who has books associated with them. Please delete the books first.");
            
            _authorsService.DeleteAuthorAsync(author);
            return Ok($"Author with ID: {id} was deleted successfully.");
        }
    }
}
