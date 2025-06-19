namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBooksService _booksService;
        private readonly ICategoriesService _categoriesService;
        private readonly IAuthorsService _authorsService;

        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };

        public BooksController(IBooksService booksService, IMapper mapper, ICategoriesService categoriesService, IAuthorsService authorsService)
        {
            _booksService = booksService;
            _mapper = mapper;
            _categoriesService = categoriesService;
            _authorsService = authorsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksService.GetBooksAsync();
            
            var data = _mapper.Map<IEnumerable<BookDetailsDto>>(books);
            
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _booksService.GetBookAsync(id);

            if (book == null)
                return NotFound();

            var dto = _mapper.Map<BookDetailsDto>(book);
            return Ok(dto);
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId)
        {
            var books = await _booksService.GetBookAsync(categoryId);
            
            if (books == null)
                return NotFound($"No books found for category ID: {categoryId}");
            
            var data = _mapper.Map<List<BookDetailsDto>>(books);
            return Ok(data);
        }


        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _booksService.GetBookAsync(authorId);
          
            if (books == null)
                return NotFound($"No books found for author ID: {authorId}");
            
            var data = _mapper.Map<List<BookDetailsDto>>(books);
            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBook([FromForm] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Cover == null)
                return BadRequest("Cover image is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Cover.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            
            var IsValidCategory = await _categoriesService.IsValidCategory(dto.CategoryId);
            if (!IsValidCategory)
                return BadRequest($"No Category Was Found With ID: {dto.CategoryId}");
            
            var IsValidAuthor = await _authorsService.IsValidAuthor(dto.AuthorId);
            if (!IsValidAuthor)
                return BadRequest($"No Author Was Found With ID: {dto.AuthorId}");
            
            using var dataStream = new MemoryStream();
            await dto.Cover.CopyToAsync(dataStream);

            var book = _mapper.Map<Book>(dto);
            book.Cover = dataStream.ToArray();

            await _booksService.CreateBookAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _booksService.GetBookAsync(id);
            if (book == null)
                return NotFound($"No Book Was Found With ID: {id}");

            if (dto.Cover == null)
                return BadRequest("Cover image is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Cover.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            var IsValidCategory = await _categoriesService.IsValidCategory(dto.CategoryId);
            if (!IsValidCategory)
                return BadRequest($"No Category Was Found With ID: {dto.CategoryId}");

            var IsValidAuthor = await _authorsService.IsValidAuthor(dto.AuthorId);
            if (!IsValidAuthor)
                return BadRequest($"No Author Was Found With ID: {dto.AuthorId}");

            using var dataStream = new MemoryStream();
            await dto.Cover.CopyToAsync(dataStream);
            
            _mapper.Map(dto, book);

            _booksService.UpdateBookAsync(book);
            return Ok(book);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _booksService.GetBookAsync(id);
            if (book == null)
            {
                return NotFound($"No Book Was Found With ID: {id}");
            }
            
            _booksService.DeleteBookAsync(book);
            return Ok($"Book with ID: {id} was deleted successfully.");
        }
    }
}
