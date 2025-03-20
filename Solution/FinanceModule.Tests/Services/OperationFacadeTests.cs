using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Services
{
    public class OperationFacadeTests
    {
        [Fact]
        public void CreateOperation_ShouldAddOperation()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var facade = new OperationFacade(accountFacade);

            var account = accountFacade.CreateAccount("Счет", 10000);
            var category = categoryFacade.CreateCategory("Зарплата", "Income");

            var operation = facade.CreateOperation("Income", account.Id, 5000, DateTime.Now, category.Id, "Зарплата");

            Assert.NotNull(operation);
            Assert.Equal(5000, operation.Amount);
        }

        [Fact]
        public void GetOperations_ShouldReturnAllOperations()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var facade = new OperationFacade(accountFacade);

            var account = accountFacade.CreateAccount("Счет", 10000);
            var category = categoryFacade.CreateCategory("Зарплата", "Income");

            facade.CreateOperation("Income", account.Id, 5000, DateTime.Now, category.Id, "Зарплата");
            facade.CreateOperation("Expense", account.Id, 2000, DateTime.Now, category.Id, "Кафе");

            var operations = facade.GetOperations();

            Assert.Equal(2, operations.Count());
        }
    }
}