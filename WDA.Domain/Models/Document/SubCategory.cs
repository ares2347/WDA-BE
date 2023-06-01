using Microsoft.EntityFrameworkCore;

namespace WDA.Domain.Models.Document;

[Keyless]
public class SubCategory
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}