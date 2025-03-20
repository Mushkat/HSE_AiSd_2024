using FinanceModule.Application.Commands;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Commands
{
    public class CreateAccountCommandTests
    {
        [Fact]
        public void Execute_ShouldCreateAccount()
        {
            var facade = new BankAccountFacade();
            var command = new CreateAccountCommand(facade, "Счет", 10000);

            var account = command.Execute();

            Assert.NotNull(account);
            Assert.Equal("Счет", account.Name);
            Assert.Equal(10000, account.Balance);
        }
    }
}