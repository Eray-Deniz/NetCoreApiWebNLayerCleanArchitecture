namespace App.Repositories.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        Task<Category?> GetCategoryWithProductsAsync(int Id);

        IQueryable<Category> GetCategoryWithProducts();
    }
}