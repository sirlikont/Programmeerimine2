using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class CategoriesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_all_categories()
        {
            // Arrange
            var url = "/api/Categories/List";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Category>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 3); // kuna SeedData lisab 3 kategooriat
        }

        [Fact]
        public async Task Get_should_return_existing_category()
        {
            // Arrange
            var url = "/api/Categories/Get?id=1";

            var category = new Category { Name = "Test Category" };
            await DbContext.AddAsync(category);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<Category>>($"/api/Categories/Get?id={category.Id}");

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(category.Id, response.Value.Id);
            Assert.Equal(category.Name, response.Value.Name);
        }

        [Fact]
        public async Task Get_should_return_notfound_for_missing_category()
        {
            // Arrange
            var url = "/api/Categories/Get?id=9999"; // id, mis eksisteerib seedis ega testis

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
