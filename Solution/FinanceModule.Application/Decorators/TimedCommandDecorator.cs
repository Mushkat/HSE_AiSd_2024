using System.Diagnostics;
using FinanceModule.Application.Interfaces;

namespace FinanceModule.Application.Decorators
{
    public class TimedCommandDecorator<T> : ICommand<T>
    {
        private readonly ICommand<T> _command;

        public TimedCommandDecorator(ICommand<T> command)
        {
            _command = command;
        }

        public T Execute()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = _command.Execute();
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            return result;
        }
    }
}