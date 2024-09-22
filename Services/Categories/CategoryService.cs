using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.Products;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using App.Services.Products.Create;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();

            var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);

            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult<CategoryDto>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var categoryAsDto = mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProdctsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var categoryAsDto = mapper.Map<CategoryWithProductsDto>(category);

            return ServiceResult<CategoryWithProductsDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProdctsAsync()
        {
            var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryAsDto = mapper.Map<List<CategoryWithProductsDto>>(category);

            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request)
        {
            var isCatNameExist = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();
            if (isCatNameExist)
            {
                return ServiceResult<CreateCategoryResponse>.Fail("Kategori ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new CreateCategoryResponse(newCategory.Id), $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            //Bu kontrol yerine Service\NotFoundFilter.cs yazıp controller da  [ServiceFilter(typeof(NotFoundFilter<Category, int>))] attribute olarak yazdık
            //var category = await categoryRepository.GetByIdAsync(id);
            //if (category is null)
            //{
            //    return ServiceResult.Fail("Güncellenecek ürün bulunamadı", HttpStatusCode.NotFound);
            //}

            var isCatNameExist = await categoryRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();
            if (isCatNameExist)
            {
                return ServiceResult.Fail("Kategori ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            var category = mapper.Map<Category>(request);
            category.Id = id;//request içerisinde id değeri olmadığı için

            categoryRepository.Update(category);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            //Bu kontrol yerine Service\NotFoundFilter.cs yazıp controller da  [ServiceFilter(typeof(NotFoundFilter<Product, int>))] attribute olarak yazdık
            //Bu yaklaşımda performanstan bir miktar ödün verdik hem yukarıda product buluyoruz hemnde NotFoundFilter içerisinde AnyAsync ile tekrar veritabanına gidiyoruz.
            //if (category is null)
            //{
            //    return ServiceResult.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            //}

            categoryRepository.Delete(category);
            await unitOfWork.SaveChangeAsync();
            //güncelleme ve silmede geriye birşey dönmüyoruz
            return ServiceResult.Success(HttpStatusCode.NoContent);

            //SaveChangeAsync işlemlerini mutlaka servis katmanından yapıyoruz. Transaction service katmanından yönetilir, repository katmanından yönetilmez.
        }
    }
}