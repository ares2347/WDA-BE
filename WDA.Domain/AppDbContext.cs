using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Email;
using WDA.Domain.Models.Ticket;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

namespace WDA.Domain;

public class AppDbContext : IdentityDbContext<User, Role,  Guid>
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CustomerTicket> CustomerTickets { get; set; }
    public DbSet<EmployeeTicket> EmployeeTickets { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasData(BuiltInData.SeedUserData());
        builder.Entity<Role>()
            .HasData(BuiltInData.SeedRoleData());
        builder.Entity<IdentityUserRole<Guid>>()
            .HasData(BuiltInData.SeedUserRoles());
        
        builder.Entity<CustomerTicket>()
            .HasOne(e => e.Requestor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<CustomerTicket>()
            .HasOne(e => e.Resolver)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<CustomerTicket>()
            .Property(e => e.ReopenReasons)
            .HasConversion(new ValueConverter<List<string>, string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new()));
        
        builder.Entity<EmployeeTicket>()
            .HasOne(e => e.Requestor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<EmployeeTicket>()
            .HasOne(e => e.Resolver)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<EmployeeTicket>()
            .Property(e => e.ReopenReasons)
            .HasConversion(new ValueConverter<List<string>, string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new()));
        
        builder.Entity<EmailTemplate>().HasData(BuiltInData.SeedEmailTemplates());
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
