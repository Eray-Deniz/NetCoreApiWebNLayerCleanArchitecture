using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class Product : BaseEntity<int>, IAuditEntity
    {
        //ef core tarafında entity class larımızı attribute ile kirletmiyoruz Bunun yerine ProductConfiguration class ında olduğu gibi ayrı bir configuration class ı üzerinde belisritoruz.Best practies için...

        public string Name { get; set; } = default!; //default olarak null değeri olmayacak anlanamına gelir. programcı yine string e null değer  verebilir, compiler uyarı verir. Ayrıca ef core da bu alan için sql server da tablo oluştururken nullable olmayacağını işaret eder.
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!; //Bir ürünün bir kategorisi olmalı o yüzden = default! null olamaz olarak işaretlendi.
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}