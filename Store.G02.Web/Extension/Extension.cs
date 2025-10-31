using Microsoft.AspNetCore.Mvc;
using Store.G02.Domain.Contracts;
using Store.G02.Persistence;
using Store.G02.Services;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Mapping;
using Store.G02.Shared.ErrorsModels;
using Store.G02.Web.Middleware;
using System.Threading.Tasks;

namespace Store.G02.Web.Extension
{
    public static class Extension
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerInService();
            services.ConfigureServices();
            


            services.AddInfraStructureService(configuration);
            services.AddApplicationServices(configuration);
            
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

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializerAsync();
            return app;
        }
        
        private static  WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }


    }
}
