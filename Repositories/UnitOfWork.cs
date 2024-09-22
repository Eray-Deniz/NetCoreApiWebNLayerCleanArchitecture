namespace App.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangeAsync() => context.SaveChangesAsync();
}