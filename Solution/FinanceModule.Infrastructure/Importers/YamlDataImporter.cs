using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace FinanceModule.Infrastructure.Importers
{
    public class YamlDataImporter : DataImporter
    {
        private readonly BankAccountFacade _accountFacade;
        private readonly CategoryFacade _categoryFacade;
        private readonly OperationFacade _operationFacade;

        public YamlDataImporter(BankAccountFacade accountFacade, CategoryFacade categoryFacade, OperationFacade operationFacade)
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
            var deserializer = new DeserializerBuilder().Build();
            var items = deserializer.Deserialize<List<Dictionary<object, object>>>(data);

            foreach (var item in items)
            {
                try
                {
                    if (item["Sort"].ToString() == "Account")
                    {
                        var account = new BankAccount
                        {
                            Id = int.Parse(item["Id"].ToString()),
                            Name = item["Name"].ToString(),
                            Balance = decimal.Parse(item["Balance"].ToString(), CultureInfo.InvariantCulture)
                        };
                        _accountFacade.CreateAccount(account.Name, account.Balance);
                    }
                    else if (item["Sort"].ToString() == "Category")
                    {
                        var category = new Category
                        {
                            Id = int.Parse(item["Id"].ToString()),
                            Type = item["Type"].ToString(),
                            Name = item["Name"].ToString()
                        };
                        _categoryFacade.CreateCategory(category.Name, category.Type);
                    }
                    else if (item["Sort"].ToString() == "Operation")
                    {
                        var operation = new Operation
                        {
                            Id = int.Parse(item["Id"].ToString()),
                            Type = item["Type"].ToString(),
                            BankAccountId = int.Parse(item["BankAccountId"].ToString()),
                            Amount = decimal.Parse(item["Amount"].ToString(), CultureInfo.InvariantCulture),
                            Date = DateTime.Parse(item["Date"].ToString()),
                            CategoryId = int.Parse(item["CategoryId"].ToString()),
                            Description = item["Description"].ToString()
                        };
                        _operationFacade.CreateOperation(operation.Type, operation.BankAccountId, operation.Amount, operation.Date, operation.CategoryId,
                            operation.Description, updateBalance: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке элемента: {ex.Message}");
                }
            }
        }
    }
}