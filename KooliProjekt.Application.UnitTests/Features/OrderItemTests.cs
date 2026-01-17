using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.OrderItems;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class OrderItemsTests : ServiceTestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetOrderItemQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_orderitem()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 10 };
            var order = new Order { OrderDate = DateTime.Now, Status = "Pending" };
            await DbContext.Products.AddAsync(product);
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 3,
                PriceAtOrder = 30
            };
            await DbContext.OrderItems.AddAsync(orderItem);
            await DbContext.SaveChangesAsync();

            var query = new GetOrderItemQuery { Id = orderItem.Id };
            var handler = new GetOrderItemQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(orderItem.Id, result.Value.Id);
            Assert.Equal(orderItem.Quantity, result.Value.Quantity);
            Assert.Equal(orderItem.PriceAtOrder, result.Value.PriceAtOrder);
            Assert.NotNull(result.Value.Product);
            Assert.Equal(product.Id, result.Value.Product.Id);
            Assert.Equal(product.Name, result.Value.Product.Name);
            Assert.Equal(product.Price, result.Value.Product.Price);
            Assert.NotNull(result.Value.Order);
            Assert.Equal(order.Id, result.Value.Order.Id);
            Assert.Equal(order.Status, result.Value.Order.Status);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_orderitem_does_not_exist(int id)
        {
            var handler = new GetOrderItemQueryHandler(DbContext);
            var query = new GetOrderItemQuery { Id = id };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            var handler = new GetOrderItemQueryHandler(DbContext);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}

