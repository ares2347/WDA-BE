using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WDA.Api.Configurations;
using WDA.Domain;
using WDA.Domain.Repositories;
using WDA.Service.Attachment;
using WDA.Service.Email;
using WDA.Service.User;
using WDA.Shared;

namespace WDA.Api;

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
        builder.Services
            .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
            .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());
        builder.Services.ConfigurationAuthentication();
        builder.Services.ConfigureJwtToken();
        builder.Services.AddAuthorization();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<IAttachmentService, AttachmentService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<UserContext>(x =>
            UserContext.Build(x.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User));

        //Email service regstration
        builder.Services.AddScoped<SmtpClient>(_ => new SmtpClient(AppSettings.Instance.Smtp.Server, 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(AppSettings.Instance.Smtp.Email, AppSettings.Instance.Smtp.Password)
        });
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<TicketRepository>();
        builder.Services.AddHangfire();
        //
        builder.Services.AddSingleton<JwtSecurityTokenHandler>();
        builder.Services.AddControllers()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        builder.Services.AddSwaggerGen(c =>
        {
            c.UseDateOnlyTimeOnlyStringConverters();
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
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
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
        app.UseStaticFiles();
        app.MapControllers();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
            IsReadOnlyFunc = _ => false
        });
        Configurations.Hangfire.RegisterRecurringJob();
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