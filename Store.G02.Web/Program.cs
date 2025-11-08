using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Persistence;
using Store.G02.Persistence.Data.Contexts;
using Store.G02.Services;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Mapping;
using Store.G02.Shared.ErrorsModels;
using Store.G02.Web.Extension;
using Store.G02.Web.Middleware;
using System.Reflection.Metadata.Ecma335;

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