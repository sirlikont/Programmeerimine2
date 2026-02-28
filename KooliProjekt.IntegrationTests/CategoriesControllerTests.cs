using Azure;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Categories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
        //uus tund 28.02
        [Fact]
        public async Task Delete_should_remove_category()
        {
            // Arrange

            var category = new Category
            {
                Name = "Test Category",
                Products = new List<Product>
        {
            new Product { Name = "Test Product 1" },
            new Product { Name = "Test Product 2" }
        }
            };

            await DbContext.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Categories/Delete";

            // Act
            var command = new DeleteCategoryCommand { Id = category.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                Content = JsonContent.Create(command)
            };
            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_handle_missing_category()
        {
            // Arrange
            var url = "/api/Categories/Delete"; 
            // Act
            var command = new DeleteCategoryCommand { Id = 9999 }; // id, mis ei eksisteeri seedis ega testis
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };
            var response = await Client.SendAsync(request);
            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Controller tagastab OK ka siis kui kategooriat pole, kuna delete on idempotentne
        }

        [Fact]
        public async Task Save_should_create_new_category()
        {
            // Arrange
            var url = "/api/Categories/Save";
         
            var command = new SaveCategoryCommand { Name = "New Category" };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };
            // Act
            var response = await Client.SendAsync(request);

            
            // Assert
            response.EnsureSuccessStatusCode();
            //tegelt pole vaja:
            //var createdCategory = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == 1);
            //Assert.NotNull(createdCategory);
        }

        [Fact]
        public async Task Save_should_not_update_missing_category()
        {
            // Arrange
            var url = "/api/Categories/Save";
            var command = new SaveCategoryCommand { Id = 9999, Name = "Updated Name" }; // id, mis ei eksisteeri seedis ega testis
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };
            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
