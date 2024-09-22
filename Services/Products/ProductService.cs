using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        //***Service katmanı dto alır ve geriye dto döndürür.

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            //bu işlem mapper ile de yapılabilir fakat manuel mapping en hızlı çalışır.
            //var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            //automapper mapping
            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return new ServiceResult<List<ProductDto>>() { Data = productAsDto };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            //List döndürürken veri yoksa null döndürme, boş liste döndür.

            var products = await productRepository.GetAll().ToListAsync();

            //Manuel mapping
            // var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            //Automapper mapping
            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            //1.sayfa - 10 kayıt => ilk 10 kayıt    skip(0).Take(10)    => 0  atla take ile 10 tane al
            //2.Sayfa - 10 kayıt => 11-20 kayıt     skip(10).Take(10)   => 10 atla take ile 10 tane al
            //3.Sayfa - 10 kayıt => 21-30 kayıt     skip(20).Take(10)   => 10 atle take ile 10 tane al

            int skip = (pageNumber - 1) * pageSize;

            var products = await productRepository.GetAll().Skip(skip).Take(pageSize).ToListAsync();

            //Manuel mapping
            //var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            //Automapper mapping
            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }

            //Manuel mapping
            //var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);

            //Automapper mapping
            var productAsDto = mapper.Map<ProductDto>(product);

            //product ta nullable uyarısı veriyordu. Yukarıda product ın nullable olma durumunu kontrol ettik, fakat compler bunu bilmiyor, o yüzden product! yazarak null olamayacağını compiler a bildiriyoruz.
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            //throw new CriticalException("Kritik seviyede bir hata meydana geldi");
            //throw new Exception("Db hatası");

            //2.way async validation
            var isProctNameExist = await productRepository.Where(x => x.Name == request.Name).AnyAsync();
            if (isProctNameExist)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            //manuel mapping
            //var product = new Product()
            //{
            //    Name = request.Name,
            //    Price = request.Price,
            //    Stock = request.Stock
            //};

            //automapper mapping
            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");

            //create ile oluşturduktan sonra $"api/products/{product.Id} ile erişecek olan url i de döndürüyoruz
        }

        //Update ve delete de data yı tekrar geri döndürmüyoruz.

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            //Clead code için kullanılan 2 durum
            //Fast fail: önce olumsuz durumları dönmektir.
            //Guard Clauses: önce gardını al.Tüm olumsuz durumları önce if ile yaz. Kesinlikle else kullanma.Else ler kodun okunabilirliğini düşürür.

            //Analyze => Calculated Code Metrics => For Solution menüsünden seçtiğimiz Cyclomatic Complexity(karmaşıklık) değeri ne kadar düşük ise o kadar iyidir.
            //Ör UpdateProductAsync metodu için Cyclomatic Complexity = 2 ise 1 tanesi fonk isminden diğeri de içerisinde 1 adet if ten kaynaklanmaktadır.
            //Fonk içerisinde mümkün olduğunda switch case veya else ifadeleri kullanılmamalıdır.

            //Bu kontrol yerine Service\NotFoundFilter.cs yazıp controller da  [ServiceFilter(typeof(NotFoundFilter<Product, int>))] attribute olarak yazdık
            //var product = await productRepository.GetByIdAsync(id);
            //if (product is null)
            //{
            //    return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            //}

            var isProctNameExist = await productRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();
            if (isProctNameExist)
            {
                return ServiceResult.Fail("Ürün ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            //manuel mapping
            //product.Name = request.Name;
            //product.Price = request.Price;
            //product.Stock = request.Stock;

            var product = mapper.Map<Product>(request);
            product.Id = id;//request içerisinde id değeri olmadığı için

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            //güncelleme ve silmede geriye birşey dönmüyoruz
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        //Bir metod 2 den fazla parametre alıyorsa requestModel oluşturmak önerilir. Bu metodda oluşturmasak da olurdu fakat örnek olsun diye UpdateProductStockRequest olarak oluşturduk.
        //public async Task<ServiceResult> UpdateStock(int Id, int quantity)
        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                return ServiceResult.Fail("Product bot found", HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        //Bu metod tek parametre id aldığı için DeleteProductRequest class yazmadık. Farklı parametrelere göre sildiğimizde DeleteProductRequest class nı yazacağız.
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            //Bu kontrol yerine Service\NotFoundFilter.cs yazıp controller da  [ServiceFilter(typeof(NotFoundFilter<Product, int>))] attribute olarak yazdık
            //Bu yaklaşımda performanstan bir miktar ödün verdik hem yukarıda product buluyoruz hemnde NotFoundFilter içerisinde AnyAsync ile tekrar veritabanına gidiyoruz.
            //if (product is null)
            //{
            //    return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            //}

            productRepository.Delete(product);
            await unitOfWork.SaveChangeAsync();
            //güncelleme ve silmede geriye birşey dönmüyoruz
            return ServiceResult.Success(HttpStatusCode.NoContent);

            //SaveChangeAsync işlemlerini mutlaka servis katmanından yapıyoruz. Transaction service katmanından yönetilir, repository katmanından yönetilmez.
        }
    }
}