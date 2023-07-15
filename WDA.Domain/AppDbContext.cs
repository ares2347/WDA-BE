using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Feedback;
using WDA.Domain.Models.Remark;
using WDA.Domain.Models.Thread;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;
using Thread = WDA.Domain.Models.Thread.Thread;

namespace WDA.Domain;

public class AppDbContext : IdentityDbContext<User, Role,  Guid>
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Remark> Remarks { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<SubTransaction> SubTransactions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Thread> Threads { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(o => o.EmployeeCode)
            .HasComputedColumnSql("CONCAT([FirstName],LEFT([LastName],1),FORMAT([Counter],'00000'))");

        builder.Entity<User>()
            .Property(x => x.Counter)
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Entity<User>()
            .Property(x => x.FullName)
            .HasComputedColumnSql("CONCAT([FirstName],SPACE(1),[LastName])");
        builder.Entity<User>()
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
