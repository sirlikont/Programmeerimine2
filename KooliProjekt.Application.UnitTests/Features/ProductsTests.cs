using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Products;
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

        [Fact]
        public async Task List_should_return_all_products()
        {
            await DbContext.Products.AddAsync(new Product { Name = "P1", Price = 10 });
            await DbContext.Products.AddAsync(new Product { Name = "P2", Price = 20 });
            await DbContext.SaveChangesAsync();

            var handler = new ListProductsQueryHandler(DbContext);

            var result = await handler.Handle(new ListProductsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_list_when_no_products_exist()
        {
            var handler = new ListProductsQueryHandler(DbContext);

            var result = await handler.Handle(new ListProductsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListProductsQueryHandler(null)
            );
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DeleteProductCommandHandler(null)
            );
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var handler = new DeleteProductCommandHandler(DbContext);
            DeleteProductCommand request = null;

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });

            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_negative(int id)
        {
            var handler = new DeleteProductCommandHandler(DbContext);
            var request = new DeleteProductCommand { Id = id };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_product()
        {
            var product = new Product { Name = "ToDelete", Price = 10 };
            await DbContext.Products.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var handler = new DeleteProductCommandHandler(DbContext);
            var request = new DeleteProductCommand { Id = product.Id };

            var result = await handler.Handle(request, CancellationToken.None);
            var count = DbContext.Products.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_product()
        {
            var handler = new DeleteProductCommandHandler(DbContext);
            var request = new DeleteProductCommand { Id = 9999 };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SaveProductCommandHandler(null)
            );
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var handler = new SaveProductCommandHandler(DbContext);
            SaveProductCommand request = null;

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });

            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_add_new_product()
        {
            var handler = new SaveProductCommandHandler(DbContext);
            var request = new SaveProductCommand
            {
                Id = 0,
                Name = "NewProduct",
                Price = 15,
                Description = "Desc",
                PhotoUrl = "img.png",
                CategoryId = 1
            };

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.Products.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Name, saved.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_product()
        {
            var product = new Product
            {
                Name = "OldName",
                Price = 10,
                Description = "OldDesc",
                PhotoUrl = "old.png",
                CategoryId = 1
            };
            await DbContext.Products.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var handler = new SaveProductCommandHandler(DbContext);
            var request = new SaveProductCommand
            {
                Id = product.Id,
                Name = "UpdatedName",
                Price = 20,
                Description = "UpdatedDesc",
                PhotoUrl = "updated.png",
                CategoryId = 1
            };

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.Products.FindAsync(product.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Name, saved.Name);
            Assert.Equal(request.Price, saved.Price);
            Assert.Equal(request.Description, saved.Description);
            Assert.Equal(request.PhotoUrl, saved.PhotoUrl);
        }
    }
}
