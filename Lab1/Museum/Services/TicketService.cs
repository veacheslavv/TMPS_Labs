using Museum.Abstractions;
using Museum.Models;

namespace Museum.Services;

// Single Responsibility Principle (Lab 0): координирует продажу, но сам не считает и не хранит
public class TicketService
{
    private readonly INotifier _notifier;
    private readonly ITicketStorage _storage;
    private readonly PriceCalculator _calc = new();

    public TicketService(INotifier notifier, ITicketStorage storage)
    {
        _notifier = notifier;
        _storage = storage;
    }

    public void Sell(string visitor, IDiscount discount)
    {
        var price = _calc.Calculate(discount);
        _storage.Save(new Ticket(visitor, discount.Name, price));
        _notifier.Notify($"{visitor}: {discount.Name}, {price:0.00} MDL");
    }

    public List<Ticket> GetTickets() => _storage.All();
    public decimal Total() => _storage.All().Sum(t => t.Price);
}
