using BLL.Common;
using BLL.DTOs.BookDTOs;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<ApiResponse<bool>> CreateBookAsync(BookCreateUpdateDTO dto)
        {
            var categoryExists = await _unitOfWork.CategoryRepository.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists) return ApiResponseHelper.Fail<bool>("Invalid Category Id", 400);

            var book = MapToEntity(dto);


            _unitOfWork.BookRepository.Add(book);
            if(await _unitOfWork.SaveChanges() > 0 )
            {
                return ApiResponseHelper.Success(true, "Book created successfully", 201);

            }

            return ApiResponseHelper.Fail<bool>("Book not created");
        }

        public async Task<ApiResponse<bool>> DeleteBookAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == id);
            if (book == null) return ApiResponseHelper.Fail<bool>("Book not found", 404);

            _unitOfWork.BookRepository.Delete(book);
            if (await _unitOfWork.SaveChanges() > 0)
            {
                return ApiResponseHelper.Success(true, "Book deleted successfully", 201);

            }

            return ApiResponseHelper.Fail<bool>("Book not deleted");
        }

        public async Task<ApiResponse<List<BookReadDTO>>> GetAllBooksAsync()
        {
  
            var books = await _unitOfWork.BookRepository.GetAllAsync(null , b => b.Category);
            var dtos = books.Select(b => MapToReadDto(b)).ToList();
            return ApiResponseHelper.Success(dtos, "Books retrieved successfully.");
        }

        public async Task<ApiResponse<List<BookReadDTO>>> GetAllBooksByCategoryAsync(int categoryId)
        {
            var categoryExists = await _unitOfWork.CategoryRepository.AnyAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                return ApiResponseHelper.Fail<List<BookReadDTO>>("Category not found.", 404);
            }

            var books = await _unitOfWork.BookRepository.GetAllAsync(b => b.CategoryId == categoryId, b => b.Category);

           
            var dtos = books.Select(b => MapToReadDto(b)).ToList();

            return ApiResponseHelper.Success(dtos, "Books retrieved successfully for this category.");
        }

        public async Task<ApiResponse<BookReadDTO>> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == id, b => b.Category);
            if (book == null) return ApiResponseHelper.Fail<BookReadDTO>("Book not found", 404);

            return ApiResponseHelper.Success(MapToReadDto(book), "Book retrieved successfully.");
        }

        public async Task<ApiResponse<List<BookReadDTO>>> GetBooksWithPaginationAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return ApiResponseHelper.Fail<List<BookReadDTO>>("Page parameters must be greater than zero.", 400);

            var books = await _unitOfWork.BookRepository.GetAllWithPaginationAsync(pageNumber, pageSize, expression: null, b => b.Category);
            var dtos = books.Select(b => MapToReadDto(b)).ToList();
            return ApiResponseHelper.Success(dtos, "Paginated books retrieved successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateBookAsync(int id, BookCreateUpdateDTO dto)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == id);
            if (book == null) return ApiResponseHelper.Fail<bool>("Book not found", 404);

            var categoryExists = await _unitOfWork.CategoryRepository.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists) return ApiResponseHelper.Fail<bool>("Invalid Category Id", 400);

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Price = dto.Price;
            book.StockQuantity = dto.StockQuantity;
            book.Description = dto.Description;
            book.CoverImageUrl = dto.CoverImageUrl;
            book.CategoryId = dto.CategoryId;


          
            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveChanges();
            return ApiResponseHelper.Success(true, "Book updated successfully.");
        }


        private static BookReadDTO MapToReadDto(Book b)
        {
           
            return new BookReadDTO {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                StockQuantity = b.StockQuantity,
                Description = b.Description,
                CoverImageUrl = b.CoverImageUrl,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name ?? string.Empty
            };
        }

        private static Book MapToEntity(BookCreateUpdateDTO book)
        {
            return new Book
            {
                Author = book.Author,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                Description = book.Description,
                CoverImageUrl = book.CoverImageUrl,
                CategoryId = book.CategoryId,
                Title = book.Title,
            };
        }

    }
}
