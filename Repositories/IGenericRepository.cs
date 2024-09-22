using System.Linq.Expressions;

namespace App.Repositories
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct //value type(int)
    {
        public Task<bool> AnyAsync(TId id);

        // IEnumarable yerine IQueryable dönmemizin sebebi direk veritabanından orderBy yapabilmemiz içindi. Diğer türlü veriyi alip sonra orderBy yapmamız gerekirdi. Asenkrona gerek yok, asenkronu ToListAsync ile birlikte Service katmanında kullanacağız.
        //IQueryable olduğunda tüm dataları çekmez where koşulundan sonra ToList dediğimiz anda dataları çeker.
        IQueryable<T> GetAll();

        //Sorguya dinamik olarak bir where koşulu yazmak için;
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        //ValueTask Task tan daha az hafızada yer tutar. Task referans tiptir, ValuTask ise Value type tır.
        ValueTask<T> GetByIdAsync(int id);

        ValueTask AddAsync(T entity);

        //Update ve delete için unitofwork ile async kullanacağız.

        void Update(T entity);

        void Delete(T entity);
    }
}