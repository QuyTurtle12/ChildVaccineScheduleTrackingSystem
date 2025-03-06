using BusinessLogic;
using Data.Base;
using Microsoft.EntityFrameworkCore;
using RazorPage.Middleware;

namespace RazorPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust timeout as needed
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAntiforgery(options => {
                options.HeaderName = "RequestVerificationToken";
            });

            // Add logging
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });
            builder.Services.AddSession();

            // Get connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("MyCnn");

            // Register DbContext with DI
            builder.Services.AddDbContext<ChildVaccineScheduleDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddApplication(builder.Configuration);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.Logger.LogInformation("Logging is configured correctly.");
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            app.UseMiddleware<JwtTokenMiddleware>();  // JWT middleware
         //   app.MapGet("/", () => Results.Redirect("/Auth/Login"));

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseAuthentication();

            app.UseSession();

            // Add logging middleware
            app.Use(async (context, next) =>
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();

                // Log incoming request details (method, path, headers, etc.)
                logger.LogInformation($"Request Method: {context.Request.Method}");
                logger.LogInformation($"Request Path: {context.Request.Path}");
                logger.LogInformation($"Request Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");

                // Log JWT token if present in the header
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    logger.LogInformation($"JWT Token: {token}");
                }

                await next.Invoke();
            });

            app.UseAuthentication(); // Enables authentication
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
