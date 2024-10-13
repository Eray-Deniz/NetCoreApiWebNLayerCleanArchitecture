using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Products
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