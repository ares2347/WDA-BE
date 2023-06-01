using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WDA.Domain;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Api.Configurations
{
    public static class Authentication
    {
        public static void ConfigurationAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<IdentityOptions>(options => { options.User.RequireUniqueEmail = true; });
        }

        public static void ConfigureJwtToken(this IServiceCollection services)
        {
            var jwtOptions = new JwtBearerOptions
            {
                SaveToken = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = AppSettings.Instance.Jwt.ValidIssuer,
                    ValidAudience = AppSettings.Instance.Jwt.ValidAudience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Instance.Jwt.Secret))
                }
            };
            services.AddSingleton(jwtOptions);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = jwtOptions.SaveToken;
                    options.TokenValidationParameters = jwtOptions.TokenValidationParameters;
                });
        }
    }
}