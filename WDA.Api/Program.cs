using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WDA.Api.Configurations;
using WDA.Domain;
using WDA.Domain.Repositories;
using WDA.Service.User;
using WDA.Shared;

namespace WDA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);
            AppSettings.Instance = appSettings;

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(appSettings.Cors.Origins.Split(';',
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.SqlServer);
            });
            builder.Services.ConfigurationAuthentication();
            builder.Services.ConfigureJwtToken();
            builder.Services.AddAuthorization();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<UserContext>(x =>
                UserContext.Build(x.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User));

            builder.Services.AddSingleton<JwtSecurityTokenHandler>();
            builder.Services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            // }
            app.UseRouting();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            //config get file name in header
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                context.Response.Headers.Add("x-frame-options", "DENY");
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                await next.Invoke();
            });
            app.Run();
        }
    }
}