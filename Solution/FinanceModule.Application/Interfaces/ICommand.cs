namespace FinanceModule.Application.Interfaces
{
    public interface ICommand<T>
    {
        T Execute();
    }
}