using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filter
{
    //ActionFilter ilgili endpoint çalışmadan önce veya çalıştıktan sonra devreye giren bri yapıdır.
    //Bir filer ctor da parametre alıyorsa (IGenericRepository<T, TId> genericRepository) olduğu gibi bunu seriviceExtension üzerinden eklememiz gerekiyor
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository)
    : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Action method çalışmadan önce

            var idValue = context.ActionArguments.TryGetValue("id", out var idAsObject) ? idAsObject : null;

            if (idAsObject is not TId id)
            {
                await next();
                return;
            }

            if (await genericRepository.AnyAsync(id))
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"data bulunamamıştır.({entityName})({actionName})");
            context.Result = new NotFoundObjectResult(result);

            //
        }
    }

    //ActionFilter ilgili endpoint çalışmadan önce veya çalıştıktan sonra devreye giren bri yapıdır.
    //Bir filer ctor da parametre alıyorsa (IGenericRepository<T, TId> genericRepository) olduğu gibi bunu seriviceExtension üzerinden eklememiz gerekiyor
    //public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : Attribute, IAsyncActionFilter where T : class where TId : struct
    //{
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        //Action method çalışmadan önce

    //        var idValue = context.ActionArguments.Values.FirstOrDefault(); //ilk parametrenin id olmasını bekliyoruz.

    //        if (idValue == null)
    //        {
    //            await next();
    //            return;
    //        }

    //        if (idValue is not TId id)
    //        {
    //            await next();
    //            return;
    //        }

    //        var anyEntity = await genericRepository.AnyAsync(id);

    //        if (!anyEntity)
    //        {
    //            var entityName = typeof(T).Name;

    //            //action method name
    //            var actionName = context.ActionDescriptor.RouteValues["action"];

    //            var result = ServiceResult.Fail($"Data bulunamadı ({entityName}) - ({actionName})");
    //            context.Result = new NotFoundObjectResult(result);
    //            return;
    //        }

    //        await next();

    //        //Action method çalıştıktan sonra
    //    }
    //}
}