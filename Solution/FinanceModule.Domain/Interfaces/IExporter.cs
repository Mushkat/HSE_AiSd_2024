using FinanceModule.Domain.Models;

namespace FinanceModule.Domain.Interfaces
{
    public interface IExporter
    {
        void Export(List<BankAccount> accounts, List<Category> categories, List<Operation> operations, string path);
    }
}