using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Products;
using Xunit;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProductsTests : ServiceTestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProductQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_product()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Desc",
                Price = 10,
                PhotoUrl = "photo.png",
                CategoryId = 1
            };
            await DbContext.Products.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var query = new GetProductQuery { Id = product.Id };
            var handler = new GetProductQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(product.Id, result.Value.Id);
            Assert.Equal(product.Name, result.Value.Name);
            Assert.Equal(product.Description, result.Value.Description);
            Assert.Equal(product.Price, result.Value.Price);
            Assert.Equal(product.PhotoUrl, result.Value.PhotoUrl);
            Assert.Equal(product.CategoryId, result.Value.CategoryId);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_product_does_not_exist(int id)
        {
            var handler = new GetProductQueryHandler(DbContext);
            var query = new GetProductQuery { Id = id };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            var handler = new GetProductQueryHandler(DbContext);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
