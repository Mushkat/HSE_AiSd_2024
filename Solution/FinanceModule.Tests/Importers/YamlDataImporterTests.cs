using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using FinanceModule.Infrastructure.Importers;
using System.IO;
using Xunit;

namespace FinanceModule.Tests.Importers
{
    public class YamlDataImporterTests
    {
        [Fact]
        public void Import_ShouldParseYamlData()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var operationFacade = new OperationFacade(accountFacade);
            var importer = new YamlDataImporter(accountFacade, categoryFacade, operationFacade);

            var yamlData = "- Type: Account\n  Id: 1\n  Name: Счет\n  Balance: 10000\n" +
                           "- Type: Category\n  Id: 1\n  Type: Income\n  Name: Зарплата\n" +
                           "- Type: Operation\n  Id: 1\n  Type: Income\n  BankAccountId: 1\n  Amount: 5000\n  Date: 2023-10-25T00:00:00\n  CategoryId: 1\n  Description: Зарплата";
            var filePath = "test.yaml";
            File.WriteAllText(filePath, yamlData);

            importer.Import(filePath);

            var accounts = accountFacade.GetAccounts();
            var categories = categoryFacade.GetCategories();
            var operations = operationFacade.GetOperations();

            Assert.Single(accounts);
            Assert.Single(categories);
            Assert.Single(operations);

            File.Delete(filePath);
        }
    }
}