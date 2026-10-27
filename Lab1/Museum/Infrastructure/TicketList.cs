using Museum.Abstractions;
using Museum.Models;

namespace Museum.Infrastructure;

public class TicketList : ITicketStorage
{
    private readonly List<Ticket> _tickets = new();
    public void Save(Ticket ticket) => _tickets.Add(ticket);
    public List<Ticket> All() => _tickets;
}
