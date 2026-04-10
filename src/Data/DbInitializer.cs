using System;
using System.Linq;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category { Name = "Electronics", Description = "Gadgets and devices" },
                new Category { Name = "Clothing", Description = "Apparel and accessories" },
                new Category { Name = "Books", Description = "Read more, know more" },
                new Category { Name = "Home & Garden", Description = "Everything for your house" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var electronicsId = categories.First(c => c.Name == "Electronics").Id;
            var clothingId = categories.First(c => c.Name == "Clothing").Id;
            var booksId = categories.First(c => c.Name == "Books").Id;

            var products = new Product[]
            {
                new Product { Name = "Gaming Mouse", Description = "High precision optical sensor", Price = 49.99m, SalesVolume = 120, CategoryId = electronicsId, CreatedTime = DateTime.Now.AddDays(-10), ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=500&q=80" },
                new Product { Name = "Mechanical Keyboard", Description = "RGB backlit keys", Price = 89.99m, SalesVolume = 85, CategoryId = electronicsId, CreatedTime = DateTime.Now.AddDays(-5), ImageUrl = "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=500&q=80" },
                new Product { Name = "Cotton T-Shirt", Description = "100% organic cotton", Price = 19.99m, SalesVolume = 300, CategoryId = clothingId, CreatedTime = DateTime.Now.AddDays(-20), ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500&q=80" },
                new Product { Name = "Denim Jeans", Description = "Classic fit", Price = 59.99m, SalesVolume = 150, CategoryId = clothingId, CreatedTime = DateTime.Now.AddDays(-15), ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=500&q=80" },
                new Product { Name = "The Great C#", Description = "Mastering modern C#", Price = 34.99m, SalesVolume = 50, CategoryId = booksId, CreatedTime = DateTime.Now.AddDays(-2), ImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=500&q=80" }
            };

            // Generate more data for pagination testing (up to 50 items)
            var random = new Random();
            for (int i = 1; i <= 45; i++)
            {
                context.Products.Add(new Product
                {
                    Name = $"Product {i}",
                    Description = $"Description for product {i}",
                    Price = (decimal)(random.NextDouble() * 100 + 10),
                    SalesVolume = random.Next(0, 500),
                    CategoryId = categories[random.Next(categories.Length)].Id,
                    CreatedTime = DateTime.Now.AddDays(-random.Next(1, 100)),
                    ImageUrl = ""
                });
            }

            foreach (var p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}
