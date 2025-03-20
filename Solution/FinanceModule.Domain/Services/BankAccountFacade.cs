using FinanceModule.Domain.Models;
using System.Collections.Generic;

namespace FinanceModule.Domain.Services
{
    public class BankAccountFacade
    {
        private List<BankAccount> _accounts = new List<BankAccount>();
        private int _nextId = 1;

        public BankAccount CreateAccount(string name, decimal balance)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название счета обязательно.");

            var account = new BankAccount { Id = _nextId++, Name = name, Balance = balance };
            _accounts.Add(account);
            return account;
        }

        // Метод для получения всех счетов
        public IEnumerable<BankAccount> GetAccounts() => _accounts;

        public BankAccount GetAccountById(int accountId)
        {
            return _accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public void UpdateBalance(int accountId, decimal newBalance)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
                throw new ArgumentException("Счет не найден.");

            account.Balance = newBalance;
        }

        public void DeleteAccount(int accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
                throw new ArgumentException("Счет не найден.");

            // Удаляем счет
            _accounts.Remove(account);
        }
    }
}