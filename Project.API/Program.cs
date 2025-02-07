
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Project.API.Helpers;
using Project.API.Services;
using Project.Core.Repositories;
using Project.Repositor;
using Project.Repositor.Data;

namespace Project.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<EmergencyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IVideoRepository, VideoRepository>();
            builder.Services.AddScoped<IVideoUploadRepository, VideoUploadRepository>();
            builder.Services.AddScoped<IEmergencyService, EmergencyService>();
            builder.Services.AddScoped<IEmergencyRequestService, EmergencyRequestService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue; // Increase max request size
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // Increase file size limit
            });

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering(); // Enable buffering for large files
                await next();
            });


            #region Update-Database
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<EmergencyContext>();
                await DbContext.Database.MigrateAsync();
                await EmergencySeed.SeedAsync(DbContext);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error Occurred During Appling Migration");
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
