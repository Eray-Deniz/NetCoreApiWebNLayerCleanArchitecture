using App.Application.Features.Categories;
using App.Application.Features.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Application.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //.Net in standart olarak ürettiği hata modelini kapat. Fluent validation ile bir custom hata modeli oluşturduk.
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            //API katmanına taşındı
            //services.AddScoped(typeof(NotFoundFilter<,>)); //2 tane generic aldığı için(T,TId) araya virgül koy <,>

            //Fluent validation ile bir custom hata modelleri oluşturuyoruz.
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Assembly.GetExecutingAssembly() ile service katmanındaki profile sınıfından miras alan tüm konfigurasyonları ekler.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //API katmanına taşındı
            //Eklenilen sıra önemli GlobalExceptionHandler da hatayı döndürdüğümüz için sona yazıldı
            //services.AddExceptionHandler<CriticalExceptionHandler>();
            //services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}