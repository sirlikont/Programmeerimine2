using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kategooriad
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" }
            );

            // Tooted (näiteks minimaalselt 10)
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smartphone", Description = "Latest model", Price = 699, CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop", Description = "Powerful gaming laptop", Price = 1200, CategoryId = 1 },
                new Product { Id = 3, Name = "Headphones", Description = "Noise-cancelling", Price = 199, CategoryId = 1 },
                new Product { Id = 4, Name = "T-Shirt", Description = "Cotton T-shirt", Price = 25, CategoryId = 2 },
                new Product { Id = 5, Name = "Jeans", Description = "Blue denim", Price = 60, CategoryId = 2 },
                new Product { Id = 6, Name = "Jacket", Description = "Winter jacket", Price = 120, CategoryId = 2 },
                new Product { Id = 7, Name = "Novel", Description = "Best-selling novel", Price = 15, CategoryId = 3 },
                new Product { Id = 8, Name = "Science Book", Description = "Educational book", Price = 30, CategoryId = 3 },
                new Product { Id = 9, Name = "Notebook", Description = "College notebook", Price = 5, CategoryId = 3 },
                new Product { Id = 10, Name = "Tablet", Description = "Portable device", Price = 350, CategoryId = 1 }
            );
        }

    }
}
