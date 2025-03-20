using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using FinanceModule.Application.Interfaces;

namespace FinanceModule.Application.Commands
{
    public class CreateAccountCommand : ICommand<BankAccount>
    {
        private readonly BankAccountFacade _facade;
        private readonly string _name;
        private readonly decimal _balance;

        public CreateAccountCommand(BankAccountFacade facade, string name, decimal balance)
        {
            _facade = facade;
            _name = name;
            _balance = balance;
        }

        public BankAccount Execute()
        {
            return _facade.CreateAccount(_name, _balance);
        }
    }
}