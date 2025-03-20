using FinanceModule.Domain.Models;
using FinanceModule.Domain.Services;
using FinanceModule.Infrastructure.Importers;
using System.IO;
using Xunit;

namespace FinanceModule.Tests.Importers
{
    public class CsvDataImporterTests
    {
        [Fact]
        public void Import_ShouldParseCsvData()
        {
            var accountFacade = new BankAccountFacade();
            var categoryFacade = new CategoryFacade();
            var operationFacade = new OperationFacade(accountFacade);
            var importer = new CsvDataImporter(accountFacade, categoryFacade, operationFacade);

            var csvData = "Account,1,Счет,10000\nCategory,1,Income,Зарплата\nOperation,1,Income,1,5000,2023-10-25,1,Зарплата";
            var filePath = "test.csv";
            File.WriteAllText(filePath, csvData);

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