using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using System.Net;
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
    }
}
