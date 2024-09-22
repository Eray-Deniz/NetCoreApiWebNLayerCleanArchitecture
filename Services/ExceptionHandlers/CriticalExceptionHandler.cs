using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Services.ExceptionHandlers
{
    // Bu handler içerisinde hatayı sms veya mail gönderebilir
    public class CriticalExceptionHandler() : IExceptionHandler //.Net 8 ile geldi
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CriticalException)
            {
                Console.WriteLine("Hata ile ilgili mail gönderildi");
            }

            //False göndererek hatayı aldım, gerekli sms mail vs işlemimi yaptım ve bir sonraki handler yani burada IExceptionHandler 'ı implemente eden GlobalExceptionHandler a gönder.
            //True döndürürsek bu hatayı ben devralıyorum anlamına gelir ve burada bir result dönmemis gerekir.
            return ValueTask.FromResult(false);
        }
    }
}