var service = new TicketService(new ConsoleNotifier(), new TicketList());

service.Sell("Ana",   new NoDiscount());
service.Sell("Ion",   new StudentDiscount());
service.Sell("Maria", new SeniorDiscount());

Console.WriteLine($"\nTotal revenue: {service.Total():0.00} MDL");  

// Open/Closed Principle (O/C): каждая скидка является отдельным классом, новая скидка добавляется новым классом без изменения старого кода
interface IDiscount
{
    string Name { get; }
    decimal Apply(decimal price);
}

class NoDiscount : IDiscount
{
    public string Name => "Standard";
    public decimal Apply(decimal price) => price;
}

class StudentDiscount : IDiscount
{
    public string Name => "Student -50%";
    public decimal Apply(decimal price) => price * 0.5m;
}

class SeniorDiscount : IDiscount
{
    public string Name => "Senior -30%";
    public decimal Apply(decimal price) => price * 0.7m;
}

// Dependency Inversion Principle (D): сервис знает только эти интерфейсы, а не конкретные классы, и получает их через конструктор

interface INotifier 
{ 
    void Notify(string message);    
}

interface ITicketStorage { 
    void Save(Ticket t); List<Ticket> All(); 
}

class ConsoleNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine(message);
}

class TicketList : ITicketStorage
{
    private readonly List<Ticket> _tickets = new();
    public void Save(Ticket t) => _tickets.Add(t);
    public List<Ticket> All() => _tickets;
}

record Ticket(string Visitor, string Discount, decimal Price);

// Single Responsibility Principle (S): у каждого класса одна задача, калькулятор только считает цену, а сервис только организует продажу
class PriceCalculator
{
    public const decimal BasePrice = 100m;
    public decimal Calculate(IDiscount discount) => discount.Apply(BasePrice);
}

class TicketService
{
    private readonly INotifier _notifier;
    private readonly ITicketStorage _storage;
    private readonly PriceCalculator _calculator = new();

    public TicketService(INotifier notifier, ITicketStorage storage)
    {
        _notifier = notifier;
        _storage = storage;
    }

    public void Sell(string visitor, IDiscount discount)
    {
        var price = _calculator.Calculate(discount);
        _storage.Save(new Ticket(visitor, discount.Name, price));
        _notifier.Notify($"{visitor}: {discount.Name}, {price:0.00} MDL");
    }

    public decimal Total() => _storage.All().Sum(t => t.Price);
}
