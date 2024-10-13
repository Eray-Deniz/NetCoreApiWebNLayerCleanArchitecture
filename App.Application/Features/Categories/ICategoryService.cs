using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;

namespace App.Application.Features.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResult<List<CategoryDto>>> GetAllListAsync();

        Task<ServiceResult<CategoryDto>> GetByIdAsync(int id);

        Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProdctsAsync(int categoryId);

        Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProdctsAsync();

        Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request);

        Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request);

        Task<ServiceResult> DeleteAsync(int id);
    }
}