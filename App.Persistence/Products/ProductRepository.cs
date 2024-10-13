using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Products
{
    //IProductRepository zaten IGenericRepository impelemente ettiği için ve IGenericRepository de GenericRepository olan 6 tane metodu burada tekrar 6 tane medtodu implemente etmeye gerek kalmadı
    public class ProductRepository(AppDbContext context) : GenericRepository<Product, int>(context), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            //metoda async ve return den sonra await ekleyebilirdik fakat metodun içerisinde tek satırda değer döndüğümüz için sync ve await in kullanılmasına gerek yok
            return Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();

            //: GenericRepository<Product>(context) context te uyarı veriyordu. Bu uyarıyı gidermek için GenericRepository içerisinde protected AppDbContext Context = context; tanımladık ve metod içerisinde de Context nesnesini kullandık.
        }
    }
}