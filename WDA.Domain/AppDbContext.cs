using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WDA.Domain.User;

namespace WDA.Domain;

public class AppDbContext : IdentityDbContext<User.User, Role,  Guid>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User.User>()
            .Property(x => x.FullName)
            .HasComputedColumnSql("CONCAT([FirstName],SPACE(1),[LastName])");
        builder.Entity<User.User>()
            .HasData(BuiltInData.SeedUserData());
        builder.Entity<Role>()
            .HasData(BuiltInData.SeedRoleData());
        builder.Entity<IdentityUserRole<Guid>>()
            .HasData(BuiltInData.SeedUserRoles());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}

/// <summary>
///     Converts <see cref="DateOnly" /> to <see cref="DateTime" /> and vice versa.
/// </summary>
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    /// <summary>
    ///     Creates a new instance of this converter.
    /// </summary>
    public DateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d))
    {
    }
}
