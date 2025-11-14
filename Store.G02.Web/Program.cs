using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Store.G02.Web.Extension;
namespace Store.G02.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.RegisterAllServices(builder.Configuration);


            var app = builder.Build();


            await app.ConfigureMiddelwares(); 


            app.Run();
        }
    }
}