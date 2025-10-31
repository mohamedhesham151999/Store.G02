using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Mapping;
using System.Reflection.Metadata;


namespace Store.G02.Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddScoped<IServiceManager, ServiceManager>();
            
            return services;
        }
        
     }  
}
