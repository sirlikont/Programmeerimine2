using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Products;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProductsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_all_products()
        {
            // Lisa mõned tooted seed data kõrvale
            await DbContext.Products.AddAsync(new Product { Name = "Prod1", Price = 10, CategoryId = 1 });
            await DbContext.Products.AddAsync(new Product { Name = "Prod2", Price = 20, CategoryId = 1 });
            await DbContext.SaveChangesAsync();

            var url = "/api/Products/List";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Product>>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);

            // Kasuta õiget property nime: Results
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_product()
        {
            var product = new Product { Name = "TestProd", Price = 15, CategoryId = 1 };
            await DbContext.Products.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Products/Get?id={product.Id}";
            var response = await Client.GetFromJsonAsync<OperationResult<ProductDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(product.Id, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_notfound_for_missing_product()
        {
            var url = "/api/Products/Get?id=9999";
            var response = await Client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Delete_should_remove_product()
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
                Description = "Test description",
                Price = 10,
                CategoryId = category.Id
            };

            await DbContext.AddAsync(product);
            await DbContext.SaveChangesAsync();

            var url = "/api/Products/Delete";

            var command = new DeleteProductCommand
            {
                Id = product.Id
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

            var deleted = await DbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            Assert.Null(deleted);
        }
        [Fact]
        public async Task Delete_should_handle_missing_product()
        {
            var url = "/api/Products/Delete";

            var command = new DeleteProductCommand
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
        public async Task Save_should_create_new_product()
        {
            // Arrange
            var category = new Category
            {
                Name = $"Category_{Guid.NewGuid()}"
            };

            await DbContext.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var url = "/api/Products/Save";

            var command = new SaveProductCommand
            {
                Name = "New Product",
                Description = "Test",
                Price = 25,
                CategoryId = category.Id
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Save_should_not_update_missing_product()
        {
            var url = "/api/Products/Save";

            var command = new SaveProductCommand
            {
                Id = 9999,
                Name = "Updated",
                Price = 15,
                CategoryId = 1
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
