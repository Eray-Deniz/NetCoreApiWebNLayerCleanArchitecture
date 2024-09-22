using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using AutoMapper;

namespace App.Services.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            //dest: product, src:CreateProductRequest (CreateProductRequest Name i ToLower yap ve bunu dest yani product Name e map le) Burada reversemap durumu yok
            CreateMap<CreateProductRequest, Product>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

            //dest: product, src:UpdateProductRequest (UpdateProductRequest Name i ToLower yap ve bunu dest yani product Name e map le) Burada reversemap durumu yok
            CreateMap<UpdateProductRequest, Product>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
        }
    }
}