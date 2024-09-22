using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;

namespace App.Services.Categories
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