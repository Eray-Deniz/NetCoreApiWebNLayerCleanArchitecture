using App.Application.Contracts.Persistence;
using App.Domain.Options;
using App.Persistence.Categories;
using App.Persistence.Interceptors;
using App.Persistence.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Persistence.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // ErayPC Sql
            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    //var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

            //    var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

            //    //connectionStrings te uyarı veriyordu. bunun null olmayacağını belirtmet için connectionStrings! şeklinde yazdık.
            //    options.UseSqlServer(connectionStrings!.SqlServer,
            //        sqlServerOptionsAction =>
            //        {
            //            sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
            //        });

            //    options.AddInterceptors(new AuditDbContextInterceptor());
            //});

            //SqlExpress
            services.AddDbContext<AppDbContext>(options =>
            {
                //var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                //connectionStrings te uyarı veriyordu. bunun null olmayacağını belirtmet için connectionStrings! şeklinde yazdık.
                options.UseSqlServer(connectionStrings!.SqlServer_Express,
                    sqlServerOptionsAction =>
                    {
                        sqlServerOptionsAction.MigrationsAssembly(typeof(PersistentAssembly).Assembly.FullName);
                    });

                options.AddInterceptors(new AuditDbContextInterceptor());
            });

            //Scoped olduğunda request geldiğinde dbcontext de nesne oluşur, response döndüğünde dbcontext de dispose olur ve nesnenin yaşam döngüsü sona erer.
            //Dbcontext in kendisi scoped olduğu için dbcontext ile beraber scoped kullanılır.Singleton veya transit olursa transaction bütünlüğü bozulur.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>)); ////2 tane generic aldığı için(T,TId) araya virgül koy <,>

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}