using App.Repositories.Products;
using App.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Products;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using App.Services.ExceptionHandlers;
using App.Services.Categories;
using Microsoft.AspNetCore.Mvc;
using App.Services.Filter;

namespace App.Services.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //.Net in standart olarak ürettiği hata modelini kapat. Fluent validation ile bir custom hata modeli oluşturduk.
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped(typeof(NotFoundFilter<,>)); //2 tane generic aldığı için(T,TId) araya virgül koy <,>

            //Fluent validation ile bir custom hata modelleri oluşturuyoruz.
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Assembly.GetExecutingAssembly() ile service katmanındaki profile sınıfından miras alan tüm konfigurasyonları ekler.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Eklenilen sıra önemli GlobalExceptionHandler da hatayı döndürdüğümüz için sona yazıldı
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}