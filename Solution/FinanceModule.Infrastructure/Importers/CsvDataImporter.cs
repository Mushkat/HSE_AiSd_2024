using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using System;
using System.IO;
using System.Linq;

namespace FinanceModule.Infrastructure.Importers
{
    public class CsvDataImporter : DataImporter
    {
        private readonly BankAccountFacade _accountFacade;
        private readonly CategoryFacade _categoryFacade;
        private readonly OperationFacade _operationFacade;

        public CsvDataImporter(BankAccountFacade accountFacade, CategoryFacade categoryFacade, OperationFacade operationFacade)
        {
            _accountFacade = accountFacade;
            _categoryFacade = categoryFacade;
            _operationFacade = operationFacade;
        }

        protected override string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        protected override void ParseData(string data)
        {
            var lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                if (parts.Length < 4)
                    continue; // Пропустить некорректные строки

                try
                {
                    if (parts[0] == "Account" && parts.Length == 4)
                    {
                        var account = new BankAccount
                        {
                            Id = int.Parse(parts[1]),
                            Name = parts[2],
                            Balance = decimal.Parse(parts[3])
                        };
                        _accountFacade.CreateAccount(account.Name, account.Balance);
                    }
                    else if (parts[0] == "Category" && parts.Length == 4)
                    {
                        var category = new Category
                        {
                            Id = int.Parse(parts[1]),
                            Type = parts[2],
                            Name = parts[3]
                        };
                        _categoryFacade.CreateCategory(category.Name, category.Type);
                    }
                    else if (parts[0] == "Operation" && parts.Length == 8)
                    {
                        var operation = new Operation
                        {
                            Id = int.Parse(parts[1]),
                            Type = parts[2],
                            BankAccountId = int.Parse(parts[3]),
                            Amount = decimal.Parse(parts[4]),
                            Date = DateTime.Parse(parts[5]),
                            CategoryId = int.Parse(parts[6]),
                            Description = parts[7]
                        };
                        _operationFacade.CreateOperation(operation.Type, operation.BankAccountId, operation.Amount, operation.Date,
                            operation.CategoryId, operation.Description, updateBalance: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке строки: {line}. Ошибка: {ex.Message}");
                }
            }
        }
    }
}