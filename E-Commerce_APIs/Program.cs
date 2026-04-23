using ECommerce.API.Services;
using ECommerce.APIs;
using ECommerce.BLL;
using ECommerce.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
namespace E_Commerce_APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();            

            builder.Services.AddDALServices(builder.Configuration);
            builder.Services.AddBLLServices();

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();



            builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
             .Configure<IOptions<JwtSettings>>((options, jwtOptions) =>
             {

                 var jwtSettings = jwtOptions.Value;
        
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                 };
             });

            builder.Services.AddScoped<JwtTokenService>();


            builder.Services.AddAuthorization(options =>
            {
                
                options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

                options.AddPolicy("UserOnly", policy =>
                policy.RequireRole("User", "Admin"));
              
            });

            builder.Services.AddOpenApi();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                await IdentitySeeder.SeedAsync(userManager, roleManager);
            }

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
