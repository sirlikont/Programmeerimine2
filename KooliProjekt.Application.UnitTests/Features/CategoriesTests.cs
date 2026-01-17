using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class CategoriesTests : ServiceTestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetCategoryQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_category()
        {
            // Arrange
            var category = new Category { Name = "Test category" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var query = new GetCategoryQuery { Id = category.Id };
            var handler = new GetCategoryQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(category.Id, result.Value.Id);
            Assert.Equal(category.Name, result.Value.Name);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_category_does_not_exist(int id)
        {
            // Arrange
            var handler = new GetCategoryQueryHandler(DbContext);
            var query = new GetCategoryQuery { Id = id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            // Arrange
            var handler = new GetCategoryQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
