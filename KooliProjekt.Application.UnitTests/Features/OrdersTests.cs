using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Orders;
using Xunit;


namespace KooliProjekt.Application.UnitTests.Features
{
    public class OrdersTests : ServiceTestBase
    {
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
    }
}

