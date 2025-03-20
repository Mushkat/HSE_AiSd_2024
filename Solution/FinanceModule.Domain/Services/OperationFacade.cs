using FinanceModule.Domain.Models;
using System.Collections.Generic;

namespace FinanceModule.Domain.Services
{
    public class OperationFacade
    {
        private List<Operation> _operations = new List<Operation>();
        private int _nextId = 1;
        private readonly BankAccountFacade _accountFacade;

        // Конструктор принимает BankAccountFacade для обновления баланса
        public OperationFacade(BankAccountFacade accountFacade)
        {
            _accountFacade = accountFacade;
        }

        public Operation CreateOperation(string type, int accountId, decimal amount, DateTime date, int categoryId, string description = "", bool updateBalance = true)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма операции должна быть положительной.");

            var operation = new Operation
            {
                Id = _nextId++,
                Type = type,
                BankAccountId = accountId,
                Amount = amount,
                Date = date,
                CategoryId = categoryId,
                Description = description
            };
            _operations.Add(operation);

            if (updateBalance)
            {
                var account = _accountFacade.GetAccountById(accountId);
                if (account == null)
                    throw new ArgumentException("Счет не найден.");

                if (type == "Income")
                    account.Balance += amount;
                else if (type == "Expense")
                    account.Balance -= amount;
                else
                    throw new ArgumentException("Неверный тип операции.");
            }

            return operation;
        }

        public IEnumerable<Operation> GetOperations() => _operations;

        public decimal CalculateBalanceDifference(DateTime startDate, DateTime endDate)
        {
            var operations = _operations
                .Where(op => op.Date >= startDate && op.Date <= endDate)
                .ToList();

            decimal totalIncome = operations
                .Where(op => op.Type == "Income")
                .Sum(op => op.Amount);

            decimal totalExpense = operations
                .Where(op => op.Type == "Expense")
                .Sum(op => op.Amount);

            return totalIncome - totalExpense;
        }
        public void DeleteOperation(int operationId)
        {
            var operation = _operations.FirstOrDefault(op => op.Id == operationId);
            if (operation == null)
                throw new ArgumentException("Операция не найдена.");

            // Откат баланса счета
            var account = _accountFacade.GetAccountById(operation.BankAccountId);
            if (account == null)
                throw new ArgumentException("Счет не найден.");

            if (operation.Type == "Income")
                account.Balance -= operation.Amount;
            else if (operation.Type == "Expense")
                account.Balance += operation.Amount;

            // Удаляем операцию
            _operations.Remove(operation);
        }
    }
}