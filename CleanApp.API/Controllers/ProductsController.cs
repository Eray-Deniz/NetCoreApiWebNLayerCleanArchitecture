using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using CleanApp.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Controllers
{
    //Bunlar miras yolu ile CustomBaseController den geldiği için kaldırdık.
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await productService.GetAllListAsync();

            //Aşğıdaki gibi her endpoint için yazmamak için CustomControllerBase sınıfını yazdık.

            //if (productResult.Status == System.Net.HttpStatusCode.NoContent)
            //{
            //    return new ObjectResult(null) { StatusCode = productResult.Status.GetHashCode() };
            //}

            //return new ObjectResult(productResult) { StatusCode = productResult.Status.GetHashCode() };

            return CreateActionResult(serviceResult);
        }

        //{pageNumber:int}/{pageSize:int}") => parametrelere girilebilcek değer tipi => root constraint
        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) => CreateActionResult(await productService.GetPagedAllListAsync(pageNumber, pageSize));

        //Yukarıdaki kodu da bu şekilde lambda ile yazabilirdik, aşamaları takip edebilmek için öyle bıraktım.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Cretae(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));

        //HttpPut tam güncellemeyi temsil ederken HttpPatch kısmi güncellemeyi temsil eder.
        //[HttpPut("updateStock")] olarak ta yapabilirdirk fakat kısmi güncellemede patch tavsiye edilir.
        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
    }
}