using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using UserApi.Models;
using RoomApi.Models;
using Microsoft.OpenApi.Models;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure MongoDB settings
        services.Configure<UserMongoDBSettings>(
            Configuration.GetSection(nameof(UserMongoDBSettings)));
        services.Configure<SkillOrTalentMongoDBSettings>(
            Configuration.GetSection("SkillOrTalentMongoDBSettings")
        );

        // Register repositories and services
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserService, UserService>();
        services.AddScoped<ISkillOrTalentRepository, SkillOrTalentRepository>();
        services.AddScoped<ISkillOrTalentService, SkillOrTalentService>();

        // Add controllers
        services.AddControllers();

        // Add Swagger services for API documentation
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RooMingle API",
                Version = "v1"
            });
        });

        // Add CORS support
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", builder =>
            {
                builder.WithOrigins("http://localhost:5173")  // Frontend URL
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();  // Allow credentials (cookies, etc.)
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(); // Enable Swagger
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RooMingle API v1"));
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Apply CORS policy before routing
        app.UseCors("AllowFrontend");

        // Middleware configuration
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Ensure CORS is before routing
        app.UseRouting();

        // Authorization (if required)
        app.UseAuthorization();

        // Configure endpoints (make sure controllers are mapped)
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
