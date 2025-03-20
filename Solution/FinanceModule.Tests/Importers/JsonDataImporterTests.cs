using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using FinanceModule.Infrastructure.Importers;
using System.IO;
using Xunit;

namespace FinanceModule.Tests.Importers
{
    public class JsonDataImporterTests
    {
        [Fact]
        public void Import_ShouldParseJsonData()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var operationFacade = new OperationFacade(accountFacade);
            var importer = new JsonDataImporter(accountFacade, categoryFacade, operationFacade);

            var jsonData = "[{\"Type\":\"Account\",\"Id\":1,\"Name\":\"Счет\",\"Balance\":10000}," +
                           "{\"Type\":\"Category\",\"Id\":1,\"Type\":\"Income\",\"Name\":\"Зарплата\"}," +
                           "{\"Type\":\"Operation\",\"Id\":1,\"Type\":\"Income\",\"BankAccountId\":1,\"Amount\":5000,\"Date\":\"2023-10-25T00:00:00\",\"CategoryId\":1,\"Description\":\"Зарплата\"}]";
            var filePath = "test.json";
            File.WriteAllText(filePath, jsonData);

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