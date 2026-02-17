using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Domain.Entities;

public class ProductTranslation
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string LanguageCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
