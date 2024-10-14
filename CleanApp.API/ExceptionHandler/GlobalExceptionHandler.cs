using App.Application;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CleanApp.API.ExceptionHandler
{
    //Uygulamadaki bütün hataları ele alan handler
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //CriticalExceptionHandler da false göndererek hatayı buraya gönderdik, burada artık true dönerek bir response dönmeli.

            var errorAsDto = ServiceResult.Fail(exception.Message, HttpStatusCode.InternalServerError);

            httpContext.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken: cancellationToken);

            return true;
        }
    }
}