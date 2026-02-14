using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;
using System.Collections.Generic;

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
    }
}
