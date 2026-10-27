using Museum.Abstractions;

namespace Museum.Infrastructure;

public class ConsoleNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine(message);
}
