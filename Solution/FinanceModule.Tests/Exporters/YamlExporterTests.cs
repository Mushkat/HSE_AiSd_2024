using FinanceModule.Domain.Models;
using FinanceModule.Infrastructure.Exporters;
using System.IO;
using Xunit;

namespace FinanceModule.Tests.Exporters
{
    public class YamlExporterTests
    {
        [Fact]
        public void Export_ShouldCreateYamlFile()
        {
            var exporter = new YamlExporter();
            var accounts = new List<BankAccount> { new BankAccount { Id = 1, Name = "Счет", Balance = 10000 } };
            var categories = new List<Category> { new Category { Id = 1, Type = "Income", Name = "Зарплата" } };
            var operations = new List<Operation> { new Operation { Id = 1, Type = "Income", BankAccountId = 1, Amount = 5000, Date = DateTime.Now, CategoryId = 1, Description = "Зарплата" } };

            foreach (var account in accounts)
                exporter.Export(account);

            foreach (var category in categories)
                exporter.Export(category);

            foreach (var operation in operations)
                exporter.Export(operation);

            var filePath = "export.yaml";
            File.WriteAllText(filePath, exporter.GetResult());

            Assert.True(File.Exists(filePath));

            var content = File.ReadAllText(filePath);
            Assert.Contains("Счет", content);
            Assert.Contains("Зарплата", content);
            Assert.Contains("5000", content);

            File.Delete(filePath);
        }
    }
}