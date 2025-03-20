using FinanceModule.Domain.Models;
using YamlDotNet.Serialization;

namespace FinanceModule.Infrastructure.Exporters
{
    public class YamlExporter : IDataExporter
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

        public string GetResult()
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(_data);
        }
    }
}