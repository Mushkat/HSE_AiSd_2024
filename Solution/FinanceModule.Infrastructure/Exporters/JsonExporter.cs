using FinanceModule.Domain.Models;
using Newtonsoft.Json;

namespace FinanceModule.Infrastructure.Exporters
{
    public class JsonExporter : IDataExporter
    {
        private readonly List<object> _data = new List<object>();

        public void Export(BankAccount account)
        {
            _data.Add(new { Sort = "Account", account.Id, account.Name, account.Balance });
        }

        public void Export(Category category)
        {
            _data.Add(new { Sort = "Category", category.Id, category.Type, category.Name });
        }

        public void Export(Operation operation)
        {
            _data.Add(new { Sort = "Operation", operation.Id, operation.Type, operation.BankAccountId, operation.Amount, operation.Date, operation.CategoryId, operation.Description });
        }

        public string GetResult() => JsonConvert.SerializeObject(_data, Formatting.Indented);
    }
}