using Microsoft.AspNetCore.Identity;
using WDA.Domain.Enums;
using WDA.Domain.User;

namespace WDA.Domain;

public static class BuiltInData
{
    private const string UserFirstName = "System";
    private const string UserLastName = "Admin";
    private const string UserEmail = "admin@email.com";
    private const string UserIdentifier = "admin";
    


    public static List<User.User> SeedUserData() => new List<User.User>
    {
        new User.User
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            UserName = UserIdentifier,
            NormalizedUserName = UserIdentifier.Normalize(),
            Email = UserEmail,
            FirstName = UserFirstName,
            LastName = UserLastName,
            EmailConfirmed = true,
            NormalizedEmail = UserEmail.Normalize(),
            PasswordHash = "AQAAAAIAAYagAAAAEMeRmOWs9W/KsBTc0NEYwk5Efsp1rjs48fPIPWSW0xhuuKWByjTRlnJXKrEmn9yPhA==",
            SecurityStamp = "3YYPM246ONSVZFAKY3TR2JSVKMX7ZM4D",
            ConcurrencyStamp = "6a9943f8-af5b-4231-9a8d-63f8c43c6e0c",
            PasswordChangeRequired = false
        }
    };

    public static List<Role> SeedRoleData() => new List<Role>
    {
        new Role
        {
            Name = RoleName.Admin,
            NormalizedName = RoleName.Admin.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000001")
        },
        new Role
        {
            Name = RoleName.Employee,
            NormalizedName = RoleName.Employee.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002")
        },
        new Role
        {
            Name = RoleName.Manager,
            NormalizedName = RoleName.Manager.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000003")
        }
    };

    public static List<IdentityUserRole<Guid>> SeedUserRoles() => new List<IdentityUserRole<Guid>>
    {
        new IdentityUserRole<Guid>
        {
            RoleId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            UserId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        }
    };
}