using System.Linq.Expressions;

namespace App.Application.Contracts.Persistence
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct //value type(int)
    {
        public Task<bool> AnyAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize);

        //Sorguya dinamik olarak bir where koşulu yazmak için;
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        //ValueTask Task tan daha az hafızada yer tutar. Task referans tiptir, ValuTask ise Value type tır.
        ValueTask<T> GetByIdAsync(int id);

        ValueTask AddAsync(T entity);

        //Update ve delete için unitofwork ile async kullanacağız.

        void Update(T entity);

        void Delete(T entity);
    }
}