using Museum.Models;

namespace Museum.Abstractions;

// Dependency Inversion Principle (Lab 0): сервис зависит от интерфейса, а не от List
public interface ITicketStorage
{
    void Save(Ticket ticket);
    List<Ticket> All();
}
