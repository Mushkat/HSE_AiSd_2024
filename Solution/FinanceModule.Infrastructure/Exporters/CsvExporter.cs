using FinanceModule.Domain.Models;
using System.Text;

namespace FinanceModule.Infrastructure.Exporters
{
    public class CsvExporter : IDataExporter
    {
        private readonly StringBuilder _csv = new StringBuilder();

        public void Export(BankAccount account)
        {
            _csv.AppendLine($"Account,{account.Id},{account.Name},{account.Balance}");
        }

        public void Export(Category category)
        {
            _csv.AppendLine($"Category,{category.Id},{category.Type},{category.Name}");
        }

        public void Export(Operation operation)
        {
            _csv.AppendLine($"Operation,{operation.Id},{operation.Type},{operation.BankAccountId},{operation.Amount},{operation.Date:yyyy-MM-dd},{operation.CategoryId},{operation.Description}");
        }

        public string GetResult() => _csv.ToString();
    }
}