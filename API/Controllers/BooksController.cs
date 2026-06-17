using BLL.Common;
using BLL.DTOs.BookDTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) => _bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BookReadDTO>>>> GetAll()
        {
            var result = await _bookService.GetAllBooksAsync();
            return Ok(result);
        }

        [HttpGet("GetWithPagination")]
        public async Task<ActionResult<ApiResponse<List<BookReadDTO>>>> GetWithPagination( int pageNumber,  int pageSize = 10)
        {
            var result = await _bookService.GetBooksWithPaginationAsync(pageNumber, pageSize);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BookReadDTO>>> GetById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result);
            return Ok(result);
        }


        [HttpGet("CategoryId/{id}")]
        public async Task<ActionResult<ApiResponse<BookReadDTO>>> GetBooksByCategoryId(int id)
        {
            var result = await _bookService.GetAllBooksByCategoryAsync(id);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<bool>>> Create(BookCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _bookService.CreateBookAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, BookCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _bookService.UpdateBookAsync(id, dto);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result);
            return Ok(result);
        }
    }
}
