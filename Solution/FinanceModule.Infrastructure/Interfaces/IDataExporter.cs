using FinanceModule.Domain.Models;

namespace FinanceModule.Infrastructure.Exporters
{
    public interface IDataExporter
    {
        void Export(BankAccount account);
        void Export(Category category);
        void Export(Operation operation);
        string GetResult();
    }
}