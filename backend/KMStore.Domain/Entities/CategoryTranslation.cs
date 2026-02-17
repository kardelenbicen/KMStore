using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Domain.Entities;

public class CategoryTranslation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string LanguageCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

