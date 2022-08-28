using System.Text.Json;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if(!context.ProductBrands.Any())
                {
                    var brandsData=File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach(var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    context.SaveChanges();
                }
                if(!context.ProductTypes.Any())
                {
                    var typesData=File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types=JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach(var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    context.SaveChanges();
                }
                if(!context.Products.Any())
                {
                    var productsData=File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products=JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach(var item in products)
                    {
                        context.Products.Add(item);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
              var logger=loggerFactory.CreateLogger<StoreContext>();
              logger.LogError(ex.Message);
            }
        }
    }
}