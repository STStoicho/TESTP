using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPresents.Data
{
    public class Present
    {
        public int Id { get; set; }
        public string CatalogNum { get; set; }
        public string Name { get; set; }
        public int CategoriesId { get; set; }
        public Category Categories { get; set; }
        public int ApliedsId { get; set; }
        public Aplied Aplieds { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public DateTime DateUpdate { get; set; } = DateTime.Now;

        public ICollection<Order> Orders { get; set; }
    }
}
