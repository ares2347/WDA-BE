using Microsoft.EntityFrameworkCore;

namespace WDA.Domain.Models.Document;

[Keyless]
public class Category
{
    public string Title { get; set; } = string.Empty;
}