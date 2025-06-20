using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Project.API.Helpers;
using Project.API.Services;
using Project.Core.Entities;
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
			builder.Services.AddScoped<IVideoTranslationService, VideoTranslationService>();
			builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddScoped<IServices, Project.Repositor.Services>();
            //builder.Services.AddScoped<UserManager<User>>();
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
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer();

			#region AI Model
			//builder.Services.AddSingleton<IAIModelServices>(sp =>
			//{
			//	var configuration = sp.GetRequiredService<IConfiguration>();
			//	var modelPath = Path.Combine(builder.Environment.ContentRootPath, "Model", "best_model.h5");
			//	var flaskServerUrl = configuration["FlaskServerUrl"];
			//	return new AIModelServices(new HttpClient(), configuration);
			//}); 
			#endregion

			builder.Services.AddSingleton<IAIModelServices>(sp =>
			{
				var httpClient = new HttpClient();
				httpClient.Timeout = TimeSpan.FromMinutes(5); // ????? ????? ??????? ??
				var configuration = sp.GetRequiredService<IConfiguration>();
				return new AIModelServices(httpClient, configuration);
			});
			//string modelPath = Path.Combine(builder.Environment.ContentRootPath, "Model", "best_model.h5");

			// ????? ?????? ?? ????? ?????? ??? ??? Constructor
			//builder.Services.AddScoped<IAIModelServices>(provider =>
			//                      new AIModelServices(modelPath));
			builder.Logging.AddConsole();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});


			var app = builder.Build();

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering(); // Enable buffering for large files
                await next();
            });
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

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

            app.UseHttpsRedirection();
			app.UseCors("AllowAll");

			// ?????? ????? ????? ??????? ?? ???? images
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.WebRootPath, "images/products")),
				RequestPath = "/images/products",
				ContentTypeProvider = new FileExtensionContentTypeProvider
				{
					Mappings = { [".mp4"] = "video/mp4" }
				},
				ServeUnknownFileTypes = true // ?????? ????? ????? ???????
			});

			app.UseStaticFiles();
			app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
// D:\Projects\EmergencService\EmergencyApp\Project.API\wwwroot\Model\best_model.h5