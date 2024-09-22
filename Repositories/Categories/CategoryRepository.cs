using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public Task<Category?> GetCategoryWithProductsAsync(int Id)
        {
            return context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == Id);
        }

        //AsQueryable döndüğümüz için async yapmadık, service katmanında tolist ile yapılacak
        //Service katmanında where şartı yazılabilir diye AsQueryable döndük
        public IQueryable<Category> GetCategoryWithProducts()
        {
            return context.Categories.Include(x => x.Products).AsQueryable();
        }
    }
}