using FinanceModule.Domain.Services;
using FinanceModule.Domain.Models;
using FinanceModule.Application.Interfaces;

namespace FinanceModule.Application.Commands
{
    public class CreateOperationCommand : ICommand<Operation>
    {
        private readonly OperationFacade _facade;
        private readonly string _type;
        private readonly int _accountId;
        private readonly decimal _amount;
        private readonly DateTime _date;
        private readonly int _categoryId;
        private readonly string _description;

        public CreateOperationCommand(
            OperationFacade facade,
            string type,
            int accountId,
            decimal amount,
            DateTime date,
            int categoryId,
            string description = "")
        {
            _facade = facade;
            _type = type;
            _accountId = accountId;
            _amount = amount;
            _date = date;
            _categoryId = categoryId;
            _description = description;
        }

        public Operation Execute()
        {
            return _facade.CreateOperation(_type, _accountId, _amount, _date, _categoryId, _description);
        }
    }
}