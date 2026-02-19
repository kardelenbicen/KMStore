using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAT { get; set; }= DateTime.UtcNow;
    public Category Category { get; set; } = null!;
    public ICollection<ProductTranslation> Translations { get; set; } = new List<ProductTranslation>();


}
