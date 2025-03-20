using FinanceModule.Application.Commands;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Commands
{
    public class CreateCategoryCommandTests
    {
        [Fact]
        public void Execute_ShouldCreateCategory()
        {
            var facade = new CategoryFacade();
            var command = new CreateCategoryCommand(facade, "Зарплата", "Income");

            var category = command.Execute();

            Assert.NotNull(category);
            Assert.Equal("Зарплата", category.Name);
            Assert.Equal("Income", category.Type);
        }
    }
}