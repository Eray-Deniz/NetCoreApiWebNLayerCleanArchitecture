using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence
{
    //(AppDbContext context) ile primary ctor kullandık
    public class GenericRepository<T, TId>(AppDbContext context) : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct //value type(int)
    {
        //protected => miras aldığımız sınıflarda kullan.
        protected AppDbContext Context = context;

        //readonly olan değişkene oluştutulduğu anda veya ctor içeirisinde değer atanabilir.
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<bool> AnyAsync(TId id) => _dbSet.AnyAsync(x => x.Id.Equals(id));

        //Fonk. içerisinde tek satır kod olduğu için lambda ile aşdağıki şekle dönüştürdük.
        //public IQueryable<T> GetAll()
        //{
        //    return _dbSet.AsQueryable();
        //}

        //Listeleme işleminde AsNoTracking yaparak efcore un listelediği verileri hafıza tutmasını engelledik.
        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();

        //predicate bir metodu işaret eden delegate dir. Herhangi bir T alır, geriye bool döndürür.
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

        public ValueTask<T> GetByIdAsync(int id) => _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate);
        }
    }
}