using Microsoft.EntityFrameworkCore;

namespace WDA.Domain.Models.User;

[Keyless]
public class Position
{
    public string Title { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}