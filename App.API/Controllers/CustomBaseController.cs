using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //Bunlar endpoint olmadığı için NonAction ile işaretledik swagger bunları endpoint olarak algılamasın.
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            if (result.Status == System.Net.HttpStatusCode.NoContent)
            {
                //Eğer result.Status 204 geldiyse ObjectResult 204 gibi çalışacak, 200 geldiyse OK gibi çalışacak, BadRequest geldiyse BadRequest gibi çalışacak
                return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };

                //Burada direk NoContent() de dönebiliriz.
                //return NoContent();
            }

            if (result.Status == System.Net.HttpStatusCode.Created)
            {
                //Port işleminde yani Created olduğunda oluşan nesnenin de erişm url sini dön.
                return Created(result.UrlAsCreated, result);
            }

            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
        }

        //Generic olmayan hali (<T> siz) ServiceResult için
        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result)
        {
            if (result.Status == System.Net.HttpStatusCode.NoContent)
            {
                //Eğer result.Status 204 geldiyse ObjectResult 204 gibi çalışacak, 200 geldiyse OK gibi çalışacak, BadRequest geldiyse BadRequest gibi çalışacak
                return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };
            }

            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
        }
    }
}