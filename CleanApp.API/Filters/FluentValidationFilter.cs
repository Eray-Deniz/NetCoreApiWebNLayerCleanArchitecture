using App.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanApp.API.Filters
{
    public class FluentValidationFilter : IAsyncActionFilter
    {
        //Kullanıcının yaptığı hatalar sonucu geriye 400 lü hataları dönüyoruz.
        //Eğer uygualama hataları varsa(veritabanı, kuyruk... vs) 500 lü hataları dönüyoruz.
        //Validasyon ile ilgili tüm hatalarda geriye 400 lü hataları döneceğiz

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                var resultModel = ServiceResult.Fail(errors);

                //Bu satırdan sonra request yoluna devam etmeyecek result u set ettiğimiz için bad request mesajı ile beraber dönecek.
                context.Result = new BadRequestObjectResult(resultModel);
                return;
            }

            //Hata yok, gelen request i controller daki endpoint e yönlendir.
            await next();
        }
    }
}