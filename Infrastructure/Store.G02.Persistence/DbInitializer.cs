using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Products;
using Store.G02.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class DbInitializer(StoreDbContext _Context) : IDbInitializer
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
    }
}
