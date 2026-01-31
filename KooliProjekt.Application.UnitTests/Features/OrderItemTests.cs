using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.OrderItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact]
        public async Task List_should_return_all_orderitems()
        {
            var product = new Product { Name = "Test Product", Price = 10 };
            var order = new Order { OrderDate = DateTime.Now, Status = "Pending" };
            await DbContext.Products.AddAsync(product);
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            await DbContext.OrderItems.AddAsync(new OrderItem { ProductId = product.Id, OrderId = order.Id, Quantity = 1, PriceAtOrder = 10 });
            await DbContext.OrderItems.AddAsync(new OrderItem { ProductId = product.Id, OrderId = order.Id, Quantity = 2, PriceAtOrder = 20 });
            await DbContext.SaveChangesAsync();

            var handler = new ListOrderItemsQueryHandler(DbContext);
            var result = await handler.Handle(new ListOrderItemsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_list_when_no_orderitems_exist()
        {
            var handler = new ListOrderItemsQueryHandler(DbContext);
            var result = await handler.Handle(new ListOrderItemsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListOrderItemsQueryHandler(null)
            );
        }

        [Fact]
        public async Task Save_should_create_new_orderitem()
        {
            var product = new Product { Name = "Test Product", Price = 10 };
            var order = new Order { OrderDate = DateTime.Now, Status = "Pending" };
            await DbContext.Products.AddAsync(product);
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var command = new SaveOrderItemCommand
            {
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 3,
                PriceAtOrder = 30
            };
            var handler = new SaveOrderItemCommandHandler(DbContext);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);

            var saved = await DbContext.OrderItems.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal(command.ProductId, saved.ProductId);
            Assert.Equal(command.OrderId, saved.OrderId);
            Assert.Equal(command.Quantity, saved.Quantity);
            Assert.Equal(command.PriceAtOrder, saved.PriceAtOrder);
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var handler = new SaveOrderItemCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await handler.Handle(null, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Delete_should_remove_existing_orderitem()
        {
            var product = new Product { Name = "Test Product", Price = 10 };
            var order = new Order { OrderDate = DateTime.Now, Status = "Pending" };
            await DbContext.Products.AddAsync(product);
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var orderItem = new OrderItem { ProductId = product.Id, OrderId = order.Id, Quantity = 1, PriceAtOrder = 10 };
            await DbContext.OrderItems.AddAsync(orderItem);
            await DbContext.SaveChangesAsync();

            var handler = new DeleteOrderItemCommandHandler(DbContext);
            var result = await handler.Handle(new DeleteOrderItemCommand { Id = orderItem.Id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(await DbContext.OrderItems.FindAsync(orderItem.Id));
        }

        [Fact]
        public async Task Delete_should_not_fail_if_orderitem_does_not_exist()
        {
            var handler = new DeleteOrderItemCommandHandler(DbContext);
            var result = await handler.Handle(new DeleteOrderItemCommand { Id = 999 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var handler = new DeleteOrderItemCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await handler.Handle(null, CancellationToken.None)
            );
        }

    }
}

