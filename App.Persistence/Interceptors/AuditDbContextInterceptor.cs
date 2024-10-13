using App.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace App.Persistence.Interceptors
{
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {
        //.Net te 3 tip delegate tipi var
        //Action => parametre alır, geriye birşey dönmez
        //Predicate => paraöetre alır, geriye bool döner
        //Func => parametre alır, geriye istenilen değeri döner

        //Swithc kullanmamak için  AddBehavior  ve ModifiedBehavior metodlarını yazdık  Dictionary ile hangi durumda hangi metodun çağrılacağını belirledik. (Bu anda new lediğimiz için readonly yaptık, readonly olan bir property i burada veya ctor içerisinde initilaze edebiliriz.)
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behavior = new()
        {
            {EntityState.Added, AddBehavior },
            {EntityState.Modified, ModifiedBehavior },
        };

        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Created = DateTime.Now;
            //ef e updated ı güncellemeye dahil etmemesini belirt
            context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Updated = DateTime.Now;
            //ef e created ı güncellemeye dahil etmemesini belirt
            context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {
                if (entityEntry.Entity is not IAuditEntity auditEntity)
                {
                    continue;
                }

                if (entityEntry.State is not (EntityState.Added or EntityState.Modified)) continue;

                //Aşağıdaki switch yerine bu kodu kullandık
                Behavior[entityEntry.State](eventData.Context, (IAuditEntity)entityEntry.Entity);

                //switch (entityEntry.State)
                //{
                //    case Microsoft.EntityFrameworkCore.EntityState.Added:

                //        auditEntity.Created = DateTime.Now;
                //        //ef e updated ı güncellemeye dahil etmemesini belirt
                //        eventData.Context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;

                //        break;

                //    case Microsoft.EntityFrameworkCore.EntityState.Modified:

                //        auditEntity.Updated = DateTime.Now;
                //        //ef e created ı güncellemeye dahil etmemesini belirt
                //        eventData.Context.Entry(auditEntity).Property(x => x.Created).IsModified = false;

                //        break;
                //}
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}