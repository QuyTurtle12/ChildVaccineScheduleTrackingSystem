using System.Reflection;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.Interface;
using Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BusinessLogic;

public static class DependencyInjection
{

    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepository();
        services.AddAutoMapper();
        services.AddServices(configuration);


    }

    public static void AddRepository(this IServiceCollection services)
    {
        services
            .AddScoped<IUOW, UOW>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ISystemAccountService, SystemAccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChildrenService, ChildrenService>();
        services.AddScoped<IRoleService, RoleService>();

        // JWT Authentication configuration
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var secretKey = configuration["Jwt:SecretKey"];
            // Log the key length for debugging
            Console.WriteLine($"[DI] JWT SecretKey Length: {secretKey?.Length}");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
        services.AddAuthorization();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<IVaccineRecordService, VaccineRecordService>();
        services.AddScoped<IVaccineService, VaccineService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPaymentService, PaymentService>();
        
    }
}