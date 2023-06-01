using Microsoft.EntityFrameworkCore;

namespace WDA.Domain.Models.User;

[Keyless]
public class Department
{
    public string Title { get; set; } = string.Empty;
}