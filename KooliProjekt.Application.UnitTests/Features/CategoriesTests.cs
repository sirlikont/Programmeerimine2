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

        // LIST HANDLER TESTID
        [Fact]
        public async Task List_should_return_all_categories()
        {
            await DbContext.Categories.AddAsync(new Category { Name = "C1" });
            await DbContext.Categories.AddAsync(new Category { Name = "C2" });
            await DbContext.SaveChangesAsync();

            var handler = new ListCategoriesQueryHandler(DbContext);

            var result = await handler.Handle(new ListCategoriesQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Value.Results.Count); // Count Results listis
        }

        [Fact]
        public async Task List_should_return_empty_list_when_no_categories_exist()
        {
            var handler = new ListCategoriesQueryHandler(DbContext);

            var result = await handler.Handle(new ListCategoriesQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListCategoriesQueryHandler(null)
            );
        }

        // DELETE HANDLER TESTID
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DeleteCategoryCommandHandler(null)
            );
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var handler = new DeleteCategoryCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(null, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Delete_should_delete_existing_category()
        {
            var category = new Category { Name = "ToDelete" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var handler = new DeleteCategoryCommandHandler(DbContext);
            var command = new DeleteCategoryCommand { Id = category.Id };

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.Categories.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_category()
        {
            var handler = new DeleteCategoryCommandHandler(DbContext);
            var command = new DeleteCategoryCommand { Id = 999 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // SAVE HANDLER TESTID
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SaveCategoryCommandHandler(null)
            );
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var handler = new SaveCategoryCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(null, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Save_should_add_new_category()
        {
            var handler = new SaveCategoryCommandHandler(DbContext);
            var command = new SaveCategoryCommand { Name = "NewCat" };

            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Categories.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(command.Name, saved.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_category()
        {
            var category = new Category { Name = "OldName" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var handler = new SaveCategoryCommandHandler(DbContext);
            var command = new SaveCategoryCommand { Id = category.Id, Name = "UpdatedName" };

            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Categories.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(command.Name, saved.Name);
        }

        // ================= VALIDATOR TESTID =================

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SaveCategoryValidator_should_fail_when_name_is_invalid(string name)
        {
            // Arrange
            var command = new SaveCategoryCommand { Name = name };
            var validator = new SaveCategoryCommandValidator(DbContext);

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task SaveCategoryValidator_should_succeed_when_name_is_valid()
        {
            // Arrange
            var command = new SaveCategoryCommand { Name = "Valid category" };
            var validator = new SaveCategoryCommandValidator(DbContext);

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.True(result.IsValid);
        }


    }
}
