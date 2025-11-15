using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            
            // Kategooriad (staatiline)
         
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" }
            );

            
            // Tooted (staatiline)
            
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

            
            // Tellimused (staatiline)

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderDate = DateTime.Now.AddDays(-0), Status = "Paid" },
                new Order { Id = 2, OrderDate = DateTime.Now.AddDays(-1), Status = "Paid" },
                new Order { Id = 3, OrderDate = DateTime.Now.AddDays(-2), Status = "Paid" },
                new Order { Id = 4, OrderDate = DateTime.Now.AddDays(-3), Status = "Paid" },
                new Order { Id = 5, OrderDate = DateTime.Now.AddDays(-4), Status = "Paid" }
            );

            
            // Tellimuse read (staatiline)
           
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, PriceAtOrder = 699 },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, PriceAtOrder = 1200 },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 3, Quantity = 1, PriceAtOrder = 199 },
                new OrderItem { Id = 4, OrderId = 2, ProductId = 4, Quantity = 3, PriceAtOrder = 25 },
                new OrderItem { Id = 5, OrderId = 3, ProductId = 5, Quantity = 1, PriceAtOrder = 60 },
                new OrderItem { Id = 6, OrderId = 3, ProductId = 6, Quantity = 1, PriceAtOrder = 120 },
                new OrderItem { Id = 7, OrderId = 4, ProductId = 7, Quantity = 2, PriceAtOrder = 15 },
                new OrderItem { Id = 8, OrderId = 4, ProductId = 8, Quantity = 1, PriceAtOrder = 30 },
                new OrderItem { Id = 9, OrderId = 5, ProductId = 9, Quantity = 4, PriceAtOrder = 5 },
                new OrderItem { Id = 10, OrderId = 5, ProductId = 10, Quantity = 1, PriceAtOrder = 350 }
            );
        }

        /*
        // KUI TEEKS NAGU ÕPETAJA:
        // Dünaamiline Generate meetod (runtime)
     
        public class SeedDataGenerator
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IList<Category> _categories = new List<Category>();
            private readonly IList<Product> _products = new List<Product>();
            private readonly IList<Order> _orders = new List<Order>();

            public SeedDataGenerator(ApplicationDbContext context)
            {
                _dbContext = context;
            }

            public void Generate()
            {
                
                // Kategooriad (dünaamiline)
                
                for (int i = 0; i < 3; i++)
                {
                    var category = new Category { Name = $"Category{i + 1}" };
                    _categories.Add(category);
                }
                _dbContext.Categories.AddRange(_categories);

               
                // Tooted (dünaamiline)
               
                int productId = 1;
                foreach (var cat in _categories)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var product = new Product
                        {
                            Name = $"Product{productId}",
                            Description = $"Description for Product{productId}",
                            Price = 10 * productId,
                            CategoryId = cat.Id
                        };
                        _products.Add(product);
                        productId++;
                    }
                }
                _dbContext.Products.AddRange(_products);

                
                // Tellimused (dünaamiline)
               
                for (int i = 0; i < 5; i++)
                {
                    var order = new Order
                    {
                        OrderDate = DateTime.Now.AddDays(-i),
                        Status = "Uus"
                    };

                    // Lisa paar tellimuse rida
                    var orderItems = new List<OrderItem>();
                    var products = _products.Take(3).ToList();
                    foreach (var p in products)
                    {
                        orderItems.Add(new OrderItem
                        {
                            ProductId = p.Id,
                            Quantity = 1,
                            PriceAtOrder = p.Price
                        });
                    }
                    order.OrderItems = orderItems;
                    _orders.Add(order);
                }
                _dbContext.Orders.AddRange(_orders);

                _dbContext.SaveChanges();
            }
        }
        */
    }
}
