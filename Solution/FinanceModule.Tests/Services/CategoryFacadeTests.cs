using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Services
{
    public class CategoryFacadeTests
    {
        [Fact]
        public void CreateCategory_ShouldAddCategory()
        {
            var facade = new CategoryFacade();
            var category = facade.CreateCategory("Зарплата", "Income");

            Assert.NotNull(category);
            Assert.Equal("Зарплата", category.Name);
            Assert.Equal("Income", category.Type);
        }

        [Fact]
        public void GetCategories_ShouldReturnAllCategories()
        {
            var facade = new CategoryFacade();
            facade.CreateCategory("Зарплата", "Income");
            facade.CreateCategory("Кафе", "Expense");

            var categories = facade.GetCategories();

            Assert.Equal(2, categories.Count());
        }
    }
}