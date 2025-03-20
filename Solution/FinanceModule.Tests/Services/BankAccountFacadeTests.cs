using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using Xunit;

namespace FinanceModule.Tests.Services
{
    public class BankAccountFacadeTests
    {
        [Fact]
        public void CreateAccount_ShouldAddAccount()
        {
            var facade = new BankAccountFacade();
            var account = facade.CreateAccount("Основной счет", 10000);

            Assert.NotNull(account);
            Assert.Equal("Основной счет", account.Name);
            Assert.Equal(10000, account.Balance);
        }

        [Fact]
        public void GetAccounts_ShouldReturnAllAccounts()
        {
            var facade = new BankAccountFacade();
            facade.CreateAccount("Счет 1", 5000);
            facade.CreateAccount("Счет 2", 10000);

            var accounts = facade.GetAccounts();

            Assert.Equal(2, accounts.Count());
        }

        [Fact]
        public void UpdateBalance_ShouldUpdateBalance()
        {
            var facade = new BankAccountFacade();
            var account = facade.CreateAccount("Счет", 5000);

            facade.UpdateBalance(account.Id, 10000);

            Assert.Equal(10000, account.Balance);
        }
    }
}