
using Microsoft.EntityFrameworkCore;
using WDA.Domain;
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
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
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