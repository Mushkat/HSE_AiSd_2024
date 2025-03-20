using FinanceModule.Console;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void CreateAccount_ShouldCreateAccount()
        {
            var accountFacade = new BankAccountFacade();
            var program = new Program();

            program.CreateAccount(accountFacade, "Счет", 10000);

            var accounts = accountFacade.GetAccounts();
            Assert.Single(accounts);
            Assert.Equal("Счет", accounts.First().Name);
            Assert.Equal(10000, accounts.First().Balance);
        }

        [Fact]
        public void CreateCategory_ShouldCreateCategory()
        {
            var categoryFacade = new CategoryFacade();
            var program = new Program();

            program.CreateCategory(categoryFacade, "Зарплата", "Income");

            var categories = categoryFacade.GetCategories();
            Assert.Single(categories);
            Assert.Equal("Зарплата", categories.First().Name);
            Assert.Equal("Income", categories.First().Type);
        }

        [Fact]
        public void CreateOperation_ShouldCreateOperation()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var operationFacade = new OperationFacade(accountFacade);
            var program = new Program();

            var account = accountFacade.CreateAccount("Счет", 10000);
            var category = categoryFacade.CreateCategory("Зарплата", "Income");

            program.CreateOperation(operationFacade, accountFacade, "Income", account.Id, 5000, DateTime.Now, category.Id, "Зарплата");

            var operations = operationFacade.GetOperations();
            Assert.Single(operations);
            Assert.Equal(5000, operations.First().Amount);
        }
    }
}