using Microsoft.AspNetCore.Identity;
using NextBuy.Models;

namespace NextBuy.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // 1. Seed Roles
        string[] roleNames = { "Admin", "Client" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Seed Admin User
        var adminEmail = "admin@nextbuy.ma";
        var adminUser = await userManager.FindByNameAsync("yahya");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "yahya",
                Email = adminEmail,
                FirstName = "Yahya",
                LastName = "Admin",
                City = "Casablanca",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "yahya123yahya");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 3. Seed Products (if empty)
        if (context.Products.Any())
        {
            return;
        }

        // Seed Categories
        var categories = new Category[]
        {
            new Category { Name = "Technologie", Description = "Derniers gadgets et appareils électroniques" },
            new Category { Name = "Mode Homme", Description = "Vêtements et accessoires pour hommes" },
            new Category { Name = "Mode Femme", Description = "Vêtements et accessoires pour femmes" },
            new Category { Name = "Maison & Déco", Description = "Tout pour votre intérieur" },
            new Category { Name = "Sport & Fitness", Description = "Équipements sportifs" }
        };

        foreach (var c in categories)
        {
            context.Categories.Add(c);
        }
        await context.SaveChangesAsync();

        // Seed Products
        var techCat = categories.First(c => c.Name == "Technologie");
        var menCat = categories.First(c => c.Name == "Mode Homme");
        var womenCat = categories.First(c => c.Name == "Mode Femme");
        var homeCat = categories.First(c => c.Name == "Maison & Déco");
        var sportCat = categories.First(c => c.Name == "Sport & Fitness");

        var products = new Product[]
        {
            new Product
            {
                Name = "Smartphone Ultra X",
                Description = "Le dernier smartphone avec un écran incroyable et une batterie longue durée.",
                Price = 8999.00m,
                Category = techCat,
                ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Casque Audio Pro",
                Description = "Son haute fidélité avec réduction de bruit active.",
                Price = 2499.00m,
                Category = techCat,
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Montre Connectée Sport",
                Description = "Suivez vos performances et restez connecté.",
                Price = 1299.00m,
                Category = techCat,
                ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Veste en Cuir Classic",
                Description = "Veste en cuir véritable, style intemporel.",
                Price = 1899.00m,
                Category = menCat,
                ImageUrl = "https://images.unsplash.com/photo-1551028919-ac66e624ecd6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Sneakers Urbaines",
                Description = "Confort et style pour la ville.",
                Price = 799.00m,
                Category = menCat,
                ImageUrl = "https://images.unsplash.com/photo-1549298916-b41d501d3772?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Robe d'été Florale",
                Description = "Légère et élégante, parfaite pour les beaux jours.",
                Price = 499.00m,
                Category = womenCat,
                ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Sac à Main Luxe",
                Description = "Cuir de qualité supérieure, finitions parfaites.",
                Price = 1599.00m,
                Category = womenCat,
                ImageUrl = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Lampe Design Moderne",
                Description = "Éclairez votre intérieur avec style.",
                Price = 399.00m,
                Category = homeCat,
                ImageUrl = "https://images.unsplash.com/photo-1507473888900-52e1adad54cd?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Plante Verte Artificielle",
                Description = "Ajoutez de la verdure sans entretien.",
                Price = 199.00m,
                Category = homeCat,
                ImageUrl = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Laptop Pro 15",
                Description = "Puissance et portabilité pour les professionnels.",
                Price = 12999.00m,
                Category = techCat,
                ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Tapis de Yoga",
                Description = "Antidérapant et écologique pour vos séances.",
                Price = 299.00m,
                Category = sportCat,
                ImageUrl = "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            },
            new Product
            {
                Name = "Haltères 5kg",
                Description = "Paire d'haltères pour le renforcement musculaire.",
                Price = 349.00m,
                Category = sportCat,
                ImageUrl = "https://images.unsplash.com/photo-1584735935682-2f2b69dff9d2?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
            }
        };

        foreach (var p in products)
        {
            context.Products.Add(p);
        }
        await context.SaveChangesAsync();
    }
}
