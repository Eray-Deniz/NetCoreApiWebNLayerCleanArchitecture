using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.Products.Dto
{
    //iki record karşılaştırıldığında içindeki property leri tek tek karşılaştırır. Fakat iki class ı karşılaştırıldığında referanslarını yani pointerlarını karşılaştırır.
    //product1 == product2 dediğimizde her ikisinin de property leri aynı ise true döner
    //record lar derlendiğinde yine class a dönüşür, class ların özelleşmiş tipleri

    //public record ProductDto
    //{
    //    public int Id { get; init; } //init => nesne örneği bri defa üretildiği anda tekrar set edilemez, değiştirilemez.
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //    public int Stock { get; init; }
    //}

    //Yukarıdaki gibi uzun yazmak yerine primary ctor içerisine yazılabilir.
    public record ProductDto(int Id, string Name, decimal Price, int Stock, int CategoryId);
}