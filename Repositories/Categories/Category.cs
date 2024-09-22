using App.Repositories.Products;

namespace App.Repositories.Categories
{
    public class Category : BaseEntity<int>, IAuditEntity
    {
        public string Name { get; set; } = default!; //null olamaz

        public List<Product>? Products { get; set; } //Bir kategorinin illaki ürünü olacak diye bir durum yok o yüzden List<Product>? nullable olarak işaretlendi.
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}