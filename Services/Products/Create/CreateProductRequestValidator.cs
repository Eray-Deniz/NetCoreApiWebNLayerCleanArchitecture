using App.Repositories.Products;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        //sync validation için kullanılıyordu
        //private readonly IProductRepository _productRepository;
        //public CreateProductRequestValidator(IProductRepository productRepository)

        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasıda olmalıdır.");
            //.Must(MustUniqueProdcutName).WithMessage("Ürün ismi veri tabanında bulunmaktadır");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0' dan büyük olmalıdır."); //price nullable olmadığı için NotNoll kontrolünü yapma.

            RuleFor(x => x.CategoryId)
               .GreaterThan(0).WithMessage("Ürün kategori değeri 0' dan büyük olmalıdır.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");

            //_productRepository = productRepository;
        }

        //sync validation
        //private bool MustUniqueProdcutName(string name)
        //{
        //    return !_productRepository.Where(x => x.Name == name).Any();

        //    //false => bir hata var
        //    //true => bir hata yok
        //}
    }
}