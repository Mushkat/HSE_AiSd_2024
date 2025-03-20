using FinanceModule.Application.Commands;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Commands
{
    public class CreateOperationCommandTests
    {
        [Fact]
        public void Execute_ShouldCreateOperation()
        {
            var accountFacade = new BankAccountFacade();
            var operationFacade = new OperationFacade(accountFacade);
            var command = new CreateOperationCommand(operationFacade, "Income", 1, 5000, DateTime.Now, 1, "Зарплата");

            var operation = command.Execute();

            Assert.NotNull(operation);
            Assert.Equal(5000, operation.Amount);
        }
    }
}