using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Orders;
using Xunit;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.UnitTests.Features
{
    public class OrdersTests : ServiceTestBase
    {
        // -------------------- GET --------------------
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetOrderQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_order()
        {
            var order = new Order { OrderDate = DateTime.Now, Status = "Pending" };
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var query = new GetOrderQuery { Id = order.Id };
            var handler = new GetOrderQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(order.Id, result.Value.Id);
            Assert.Equal(order.Status, result.Value.Status);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_order_does_not_exist(int id)
        {
            var handler = new GetOrderQueryHandler(DbContext);
            var query = new GetOrderQuery { Id = id };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            var handler = new GetOrderQueryHandler(DbContext);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // -------------------- LIST --------------------
        [Fact]
        public async Task List_should_return_all_orders()
        {
            await DbContext.Orders.AddAsync(new Order { Status = "Pending" });
            await DbContext.Orders.AddAsync(new Order { Status = "Completed" });
            await DbContext.SaveChangesAsync();

            var handler = new ListOrdersQueryHandler(DbContext);

            var result = await handler.Handle(new ListOrdersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_list_when_no_orders_exist()
        {
            var handler = new ListOrdersQueryHandler(DbContext);

            var result = await handler.Handle(new ListOrdersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListOrdersQueryHandler(null)
            );
        }

        // -------------------- DELETE --------------------
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DeleteOrderCommandHandler(null)
            );
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var handler = new DeleteOrderCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(null, CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_should_not_fail_if_order_does_not_exist(int id)
        {
            var handler = new DeleteOrderCommandHandler(DbContext);
            var command = new DeleteOrderCommand { Id = id };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_order()
        {
            var order = new Order { Status = "Pending" };
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var handler = new DeleteOrderCommandHandler(DbContext);
            var command = new DeleteOrderCommand { Id = order.Id };

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.Orders.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        // -------------------- SAVE --------------------
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SaveOrderCommandHandler(null)
            );
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var handler = new SaveOrderCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(null, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Save_should_add_new_order()
        {
            var handler = new SaveOrderCommandHandler(DbContext);
            var command = new SaveOrderCommand { Status = "Pending" };

            var result = await handler.Handle(command, CancellationToken.None);
            var savedOrder = await DbContext.Orders.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedOrder);
            Assert.Equal(command.Status, savedOrder.Status);
        }

        [Fact]
        public async Task Save_should_update_existing_order()
        {
            var order = new Order { Status = "Pending" };
            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();

            var handler = new SaveOrderCommandHandler(DbContext);
            var command = new SaveOrderCommand { Id = order.Id, Status = "Completed" };

            var result = await handler.Handle(command, CancellationToken.None);
            var updatedOrder = await DbContext.Orders.FindAsync(order.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedOrder);
            Assert.Equal("Completed", updatedOrder.Status);
        }
    }
}
