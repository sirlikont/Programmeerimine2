using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Orders;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class OrdersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_all_orders()
        {
            // Lisame kaks tellimust koos vähemalt ühe OrderItem-iga
            var order1 = new Order
            {
                OrderDate = System.DateTime.Now,
                Status = "Paid",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1, PriceAtOrder = 10 }
                }
            };

            var order2 = new Order
            {
                OrderDate = System.DateTime.Now,
                Status = "Paid",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 2, Quantity = 2, PriceAtOrder = 20 }
                }
            };

            await DbContext.Orders.AddAsync(order1);
            await DbContext.Orders.AddAsync(order2);
            await DbContext.SaveChangesAsync();

            var url = "/api/Orders/List";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Order>>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_order()
        {
            var order = new Order
            {
                OrderDate = System.DateTime.Now,
                Status = "Paid",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1, PriceAtOrder = 10 }
                }
            };

            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Orders/Get?id={order.Id}";
            var response = await Client.GetFromJsonAsync<OperationResult<Order>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(order.Id, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_notfound_for_missing_order()
        {
            var url = "/api/Orders/Get?id=9999"; // ID, mis seed datast ei eksisteeri
            var response = await Client.GetAsync(url);

            // Kontrollime, et controller tagastab NotFound (404)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        // 28.02 tund
        [Fact]
        public async Task Delete_should_remove_order()
        {
            // Arrange

            var category = new Category
            {
                Name = $"Category_{Guid.NewGuid()}"
            };

            await DbContext.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Test Product",
                Price = 10,
                CategoryId = category.Id
            };

            await DbContext.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var order = new Order
            {
                Status = "Paid",
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                ProductId = product.Id,
                Quantity = 1,
                PriceAtOrder = 10
            }
        }
            };

            await DbContext.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var url = "/api/Orders/Delete";

            var command = new DeleteOrderCommand
            {
                Id = order.Id
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deleted = await DbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            Assert.Null(deleted);
        }
        [Fact]
        public async Task Delete_should_handle_missing_order()
        {
            var url = "/api/Orders/Delete";

            var command = new DeleteOrderCommand
            {
                Id = 9999
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Save_should_create_new_order()
        {
            var url = "/api/Orders/Save";

            var command = new SaveOrderCommand
            {
                Status = "Paid",
                OrderDate = DateTime.Now
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Save_should_not_update_missing_order()
        {
            var url = "/api/Orders/Save";

            var command = new SaveOrderCommand
            {
                Id = 9999,
                Status = "Updated"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
