using BLL.Common;
using BLL.DTOs.BookDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<ApiResponse<List<BookReadDTO>>> GetAllBooksAsync();
        Task<ApiResponse<List<BookReadDTO>>> GetAllBooksByCategoryAsync(int categoryId);

        Task<ApiResponse<List<BookReadDTO>>> GetBooksWithPaginationAsync(int pageNumber, int pageSize);
        Task<ApiResponse<BookReadDTO>> GetBookByIdAsync(int id);
        Task<ApiResponse<bool>> CreateBookAsync(BookCreateUpdateDTO dto);
        Task<ApiResponse<bool>> UpdateBookAsync(int id, BookCreateUpdateDTO dto);
        Task<ApiResponse<bool>> DeleteBookAsync(int id);
    }
}
