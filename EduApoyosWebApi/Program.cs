using EduApoyosApplication;
using EduApoyosApplication.Implementation;
using EduApoyosApplication.Settings;
using EduApoyosCommon.Interface;
using EduApoyosInfrastructure.Persistence;
using EduApoyosInfrastructure.Repository;
using EduApoyosWebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IUsuarioTokenRepository, UsuarioTokenRepository>();
        builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
        builder.Services.AddScoped<IEstudianteApplication, EstudianteApplication>();
        builder.Services.AddScoped<IAuthApplication, AuthApplication>();
        builder.Services.AddScoped<ISolicitudApplication, SolicitudApplication>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IJwtService, JwtService>();

        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("Jwt"));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AngularPolicy",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        var jwtSettings =builder.Configuration
            .GetSection("Jwt")
            .Get<JwtSettings>() ?? throw new InvalidOperationException("Configuración JWT no encontrada.");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                        RoleClaimType = ClaimTypes.Role
                    };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseCors("AngularPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}