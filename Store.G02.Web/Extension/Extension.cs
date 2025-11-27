using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Persistence;
using Store.G02.Persistence.Identity.Contexts;
using Store.G02.Services;
using Store.G02.Shared;
using Store.G02.Shared.ErrorsModels;
using Store.G02.Web.Middleware;
using System.Text;


namespace Store.G02.Web.Extension
{
    public static class Extension
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerInService();
            services.AddIdentityInService();
            services.ConfigureServices();

            services.AddInfraStructureService(configuration);
            services.AddApplicationServices(configuration);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.AddAuthenticationService(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            return services;

        }

        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOption = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultAuthenticateScheme = "Bearer";

            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    #region to check if token is valid or not 

                    ValidateIssuer = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOption.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecurityKey))

                    #endregion
                };
            });

            return services;

        }
        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();

            return services;

        }
        private static IServiceCollection AddSwaggerInService(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddIdentityInService(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                 
            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<IdentityStoreDbContext>();

            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(m => m.Value.Errors.Any())
                                   .Select(m => new ValidationError()
                                   {
                                       Field = m.Key,
                                       Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                   });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
        public static async Task<WebApplication> ConfigureMiddelwares(this WebApplication app)
        {
            await app.InitializeDatabaseAsync();

            app.UseGlobalErrorHandling();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();  

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializerAsync();
            await dbInitializer.InitializeIdentityAsync();
            return app;
        }
        
        private static  WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }


    }
}
