using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FinanceModule.Infrastructure.Importers
{
    public class JsonDataImporter : DataImporter
    {
        private readonly BankAccountFacade _accountFacade;
        private readonly CategoryFacade _categoryFacade;
        private readonly OperationFacade _operationFacade;

        public JsonDataImporter(BankAccountFacade accountFacade, CategoryFacade categoryFacade, OperationFacade operationFacade)
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
            var items = JsonConvert.DeserializeObject<List<dynamic>>(data);
            foreach (var item in items)
            {
                if (item.Sort == "Account")
                {
                    var account = new BankAccount
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Balance = item.Balance
                    };
                    _accountFacade.CreateAccount(account.Name, account.Balance);
                }
                else if (item.Sort == "Category")
                {
                    var category = new Category
                    {
                        Id = item.Id,
                        Type = item.Type,
                        Name = item.Name
                    };
                    _categoryFacade.CreateCategory(category.Name, category.Type);
                }
                else if (item.Sort == "Operation")
                {
                    var operation = new Operation
                    {
                        Id = item.Id,
                        Type = item.Type,
                        BankAccountId = item.BankAccountId,
                        Amount = item.Amount,
                        Date = item.Date,
                        CategoryId = item.CategoryId,
                        Description = item.Description
                    };
                    _operationFacade.CreateOperation(operation.Type, operation.BankAccountId, operation.Amount, operation.Date, 
                        operation.CategoryId, operation.Description, updateBalance: false);
                }
            }
        }
    }
}