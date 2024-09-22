using App.Repositories.Categories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        //.net 8 den itibaren primary Consructor geldi. Class ismi içerisine primary contructor olarak Quick Actions tan taşındı.Primary constructor: AppDbContext(DbContextOptions<AppDbContext> options)

        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        //{
        //}
        public DbSet<Product> Products { get; set; } = default!;

        public DbSet<Category> Categories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bu repository(Assembly.GetExecutingAssembly()) içerisindeki IEntityTypeConfiguration ı implemente etmişş olan tüm sınıfları al.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}