namespace Museum.Abstractions;

// Dependency Inversion Principle (Lab 0): сервис зависит от интерфейса, а не от Console
public interface INotifier
{
    void Notify(string message);
}
