using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.OrderItems;
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
    public class OrderItemsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_orderitems()
        {
            var url = "/api/OrderItems/List";

            var response = await Client.GetFromJsonAsync<PagedResult<OrderItem>>(url);

            Assert.NotNull(response);
            Assert.True(response.Results.Count >= 0);
        }

        [Fact]
        public async Task Get_should_return_existing_orderitem()
        {
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
                Status = "Paid"
            };

            await DbContext.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 1,
                PriceAtOrder = 10
            };

            await DbContext.AddAsync(orderItem);
            await DbContext.SaveChangesAsync();

            var url = $"/api/OrderItems/Get?id={orderItem.Id}";

            var response = await Client.GetFromJsonAsync<OrderItem>(url);

            Assert.NotNull(response);
            Assert.Equal(orderItem.Id, response.Id);
        }

        [Fact]
        public async Task Get_should_return_notfound_for_missing_orderitem()
        {
            var url = "/api/OrderItems/Get?id=9999";

            var response = await Client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_orderitem()
        {
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
                Status = "Paid"
            };

            await DbContext.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 1,
                PriceAtOrder = 10
            };

            await DbContext.AddAsync(orderItem);
            await DbContext.SaveChangesAsync();

            var url = "/api/OrderItems/Delete";

            var command = new DeleteOrderItemCommand
            {
                Id = orderItem.Id
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deleted = await DbContext.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderItem.Id);

            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_handle_missing_orderitem()
        {
            var url = "/api/OrderItems/Delete";

            var command = new DeleteOrderItemCommand
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
        public async Task Save_should_create_new_orderitem()
        {
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
                Status = "Paid"
            };

            await DbContext.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var url = "/api/OrderItems/Save";

            var command = new SaveOrderItemCommand
            {
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 1,
                PriceAtOrder = 10
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}