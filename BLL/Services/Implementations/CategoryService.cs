using BLL.Common;
using BLL.DTOs.CategoryDTOs;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; } 
        public async Task<ApiResponse<bool>> CreateCategoryAsync(CategoryCreateUpdateDTO dto)
        {
            var category = new Category { Name = dto.Name };
            _unitOfWork.CategoryRepository.Add(category);
            if (await _unitOfWork.SaveChanges() > 0)
            {
                return ApiResponseHelper.Success(true, "Category created successfully", 201);
            }
            return ApiResponseHelper.Fail<bool>("Category not created");

        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return ApiResponseHelper.Fail<bool>("Category not found", 404);

            _unitOfWork.CategoryRepository.Delete(category);
           if( await _unitOfWork.SaveChanges() > 0)
            {
                return ApiResponseHelper.Success(true, "Category deleted successfully.");

            }

            return ApiResponseHelper.Fail<bool>("Category not deleted");


        }

        public async Task<ApiResponse<List<CategoryReadDTO>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            if(categories != null)
            {
                var dtos = categories.Select(c => new CategoryReadDTO { Id = c.Id, Name = c.Name }).ToList();
                return ApiResponseHelper.Success(dtos, "Categories retrieved successfully.");

            }

            return ApiResponseHelper.Fail<List<CategoryReadDTO>>("Categories not retrieved ");
        }
        

        public async Task<ApiResponse<CategoryReadDTO>> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return ApiResponseHelper.Fail<CategoryReadDTO>("Category not found", 404);

            var dto = new CategoryReadDTO { Id = category.Id, Name = category.Name };
            return ApiResponseHelper.Success(dto, "Category retrieved successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateCategoryAsync(int id, CategoryCreateUpdateDTO dto)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return ApiResponseHelper.Fail<bool>("Category not found", 404);

            category.Name = dto.Name;
            
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChanges();
            return ApiResponseHelper.Success(true, "Category updated successfully.");
        }
    }
}
