using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;

namespace App.Services.Categories
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();

            CreateMap<Category, CategoryWithProductsDto>().ReverseMap(); // Category ve CategoryWithProductsDto içerisindeki Products ile otomatik mapping yapacak.

            //dest: product, src:CreateProductRequest (CreateProductRequest Name i ToLower yap ve bunu dest yani product Name e map le) Burada reversemap durumu yok
            CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

            //dest: product, src:UpdateProductRequest (UpdateProductRequest Name i ToLower yap ve bunu dest yani product Name e map le) Burada reversemap durumu yok
            CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
        }
    }
}