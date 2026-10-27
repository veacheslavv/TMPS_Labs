using Museum.Abstractions;
using Museum.Factories;
using Museum.Infrastructure;
using Museum.Pricing;
using Museum.Services;

// --- Singleton: единственный экземпляр конфигурации музея ---
var config = MuseumConfig.Instance;
Console.WriteLine($"Museum: {config.MuseumName}");
Console.WriteLine($"Base price: {config.BasePrice} MDL");

// --- Продажа билетов (SOLID из Lab 0) ---
Console.WriteLine("\n=== TICKET SALES ===");
var service = new TicketService(new ConsoleNotifier(), new TicketList());

service.Sell("Ana",   new NoDiscount());
service.Sell("Ion",   new StudentDiscount());
service.Sell("Maria", new SeniorDiscount());

Console.WriteLine($"\nTotal revenue: {service.Total():0.00} MDL");

// --- Prototype: клонируем билет ---
Console.WriteLine("\n=== PROTOTYPE ===");
var original = service.GetTickets()[0];
Console.WriteLine($"Original: {original}");

var clone = original.Clone();
clone.Visitor = "Ana (copy for archive)";
Console.WriteLine($"Clone:    {clone}");
Console.WriteLine($"Same object? {ReferenceEquals(original, clone)}");

// --- Abstract Factory: разные сезоны создают разные наборы ---
Console.WriteLine("\n=== ABSTRACT FACTORY ===");
ServeVisitor("Elena", new RegularSeasonFactory());
ServeVisitor("Petru", new NightMuseumFactory());

void ServeVisitor(string name, ISeasonFactory factory)
{
    var discount = factory.CreateDiscount();
    var greeting = factory.CreateGreeting(name);
    var price = discount.Apply(MuseumConfig.Instance.BasePrice);
    Console.WriteLine(greeting);
    Console.WriteLine($"  {discount.Name}: {price:0.00} MDL");
}

// --- Singleton: проверка что экземпляр один ---
Console.WriteLine("\n=== SINGLETON CHECK ===");
Console.WriteLine($"Same instance? {ReferenceEquals(config, MuseumConfig.Instance)}");
