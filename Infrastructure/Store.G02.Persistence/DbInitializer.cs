using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Entities.Order;
using Store.G02.Domain.Entities.Products;
using Store.G02.Persistence.Data.Contexts;
using Store.G02.Persistence.Identity.Contexts;
using Store.G02.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class DbInitializer(StoreDbContext _Context, IdentityStoreDbContext _identityContext, UserManager<AppUser> _userManager,  RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        
        public async Task InitializerAsync()
        {
            #region SummaryOfTheFunction
            // This Function Create Database incase it is not found 
            // Also update database with last migration found
            // Then Data seeding 
            #endregion

            ////Create Db
            ////Update Db
            if (_Context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _Context.Database.MigrateAsync();
            }



            #region SummaryOfDataSeeding
            //// 1. Read All data from Json File 'brands.json'
            ////2. convert the json string to <List>ProductBrand>
            ////3.Add List To The DataBase
            #endregion


            ////Data Seeding

            // Product Brands
            if (!_Context.DeliveryMethods.Any())
            {
                var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                if (deliveryMethods is not null && deliveryMethods.Count > 0)
                {
                    await _Context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                }
            }



            // Product Brands
            if (!_Context.ProductBrands.Any())
            {
                var branddata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(branddata);
                if (brands is not null && brands.Count > 0)
                {
                    await _Context.ProductBrands.AddRangeAsync(brands);
                }
            }



            //// ProductTypes
            if (!_Context.ProductTypes.Any())
            {
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                if (types is not null && types.Count > 0)
                {
                    await _Context.ProductTypes.AddRangeAsync(types);
                }
            }



            //// Product
            if (!_Context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products is not null && products.Count > 0)
                {
                    await _Context.Products.AddRangeAsync(products);
                }
            }



            await _Context.SaveChangesAsync();



        }

        public async Task InitializeIdentityAsync()
          {
            if (_identityContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _identityContext.Database.MigrateAsync();
            }


            // Data Seeding

            if (!_identityContext.Roles.Any())
            {
               await _roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
               await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }

            if (!_identityContext.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01234556778"
                };


                var Admin = new AppUser()
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01234556778"
                };


                await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                await _userManager.CreateAsync(Admin, "P@ssw0rd");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(Admin, "Admin");

            }



        }


    }
}
