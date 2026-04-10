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
                // 如果已有資料，我們先刪除舊商品，確保圖片與設計稿同步
                context.Products.RemoveRange(context.Products);
                context.Categories.RemoveRange(context.Categories);
                context.SaveChanges();
            }

            var categories = new Category[]
            {
                new Category { Name = "Outerwear", Description = "Coats and jackets" },
                new Category { Name = "Knitwear", Description = "Sweaters and cardigans" },
                new Category { Name = "Tailoring", Description = "Blazers and suits" },
                new Category { Name = "Accessories", Description = "Bags and small goods" },
                new Category { Name = "Loungewear", Description = "Comfortable home wear" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var outerwearId = categories.First(c => c.Name == "Outerwear").Id;
            var knitwearId = categories.First(c => c.Name == "Knitwear").Id;
            var tailoringId = categories.First(c => c.Name == "Tailoring").Id;
            var accessoriesId = categories.First(c => c.Name == "Accessories").Id;

            // 使用 Figma MCP 提供的最新 Asset URLs
            var products = new Product[]
            {
                new Product { Name = "Archival Overcoat", Description = "Double-faced Wool", Price = 840m, SalesVolume = 12, CategoryId = outerwearId, Material = "Double-faced Wool", Color = "#e9e2d4", CreatedTime = DateTime.Now.AddDays(-10), ImageUrl = "https://www.figma.com/api/mcp/asset/0571b8f5-e7a8-439b-9f10-c2ff0f6dd815" },
                new Product { Name = "Knit Structure T-01", Description = "Loro Piana Cashmere", Price = 320m, SalesVolume = 8, CategoryId = knitwearId, Material = "Loro Piana Cashmere", Color = "#323233", CreatedTime = DateTime.Now.AddDays(-5), ImageUrl = "https://www.figma.com/api/mcp/asset/5db07938-c2ed-4e66-bde1-d68b0f597629" },
                new Product { Name = "Sculptural Blouse", Description = "Italian Poplin", Price = 450m, SalesVolume = 15, CategoryId = outerwearId, Material = "Italian Poplin", Color = "#ffffff", CreatedTime = DateTime.Now.AddDays(-20), ImageUrl = "https://www.figma.com/api/mcp/asset/c6730dd6-dffe-4ed1-80e3-7194a5d6e5c7" },
                new Product { Name = "Raw Selvedge Denim", Description = "14oz Japanese Denim", Price = 280m, SalesVolume = 4, CategoryId = outerwearId, Material = "Raw Silk", Color = "#323233", CreatedTime = DateTime.Now.AddDays(-15), ImageUrl = "https://www.figma.com/api/mcp/asset/212f291b-6c96-426e-8593-d2dd7c9ce4b9" },
                new Product { Name = "Atelier Blazer No. 4", Description = "Bespoke Tailoring", Price = 1100m, SalesVolume = 11, CategoryId = tailoringId, Material = "Sourced Cashmere", Color = "#536254", CreatedTime = DateTime.Now.AddDays(-2), ImageUrl = "https://www.figma.com/api/mcp/asset/ff7bee20-b948-404d-b323-acd264fe6fb2" },
                new Product { Name = "Structured Tote", Description = "Vegetable Tanned Leather", Price = 675m, SalesVolume = 20, CategoryId = accessoriesId, Material = "Raw Silk", Color = "#323233", CreatedTime = DateTime.Now.AddDays(-1), ImageUrl = "https://www.figma.com/api/mcp/asset/454f4ac4-164b-468e-8975-cfe23fdd9669" }
            };

            var materials = new[] { "Sourced Cashmere", "Organic Wool", "Raw Silk", "Double-faced Wool", "Loro Piana Cashmere", "Italian Poplin" };
            var colors = new[] { "#323233", "#536254", "#dbdad9", "#e9e2d4", "#ffffff" };

            foreach (var p in products)
            {
                context.Products.Add(p);
            }

            // 生成隨機商品補充網格
            var random = new Random();
            for (int i = 1; i <= 12; i++)
            {
                context.Products.Add(new Product
                {
                    Name = $"Essential Item {i}",
                    Description = $"Description for essential item {i}",
                    Price = (decimal)(random.NextDouble() * 1000 + 100),
                    SalesVolume = random.Next(0, 100),
                    CategoryId = categories[random.Next(categories.Length)].Id,
                    Material = materials[random.Next(materials.Length)],
                    Color = colors[random.Next(colors.Length)],
                    CreatedTime = DateTime.Now.AddDays(-random.Next(1, 100)),
                    ImageUrl = ""
                });
            }

            context.SaveChanges();
        }
    }
}
