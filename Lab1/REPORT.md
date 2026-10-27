# Laboratory Work 1: Creational Design Patterns

## Topic and Goal

The semester project is a museum management system. In Laboratory Work 0 the system was built with three SOLID principles in a single file. In this laboratory work the project grows: the code is split into a classic C# file structure and three creational design patterns are applied: Singleton, Prototype and Abstract Factory. The goal is to show how objects can be created in a controlled and flexible way without scattering `new` calls across the code.

## Project Overview

The project is written in C# (.NET 8) and split into folders by responsibility. The folder `Abstractions` holds the interfaces, `Models` holds the data classes, `Pricing` holds the discount implementations, `Factories` holds the season factories, `Infrastructure` holds the console and storage implementations, and `Services` holds the business logic and the museum configuration. The entry point is `Program.cs`, which connects all parts and demonstrates each pattern.

```
Museum/
├── Program.cs
├── Models/
│   └── Ticket.cs
├── Abstractions/
│   ├── IDiscount.cs
│   ├── INotifier.cs
│   ├── ITicketStorage.cs
│   └── ISeasonFactory.cs
├── Pricing/
│   ├── NoDiscount.cs
│   ├── StudentDiscount.cs
│   ├── SeniorDiscount.cs
│   └── NightDiscount.cs
├── Factories/
│   ├── RegularSeasonFactory.cs
│   └── NightMuseumFactory.cs
├── Infrastructure/
│   ├── ConsoleNotifier.cs
│   └── TicketList.cs
└── Services/
    ├── MuseumConfig.cs
    ├── PriceCalculator.cs
    └── TicketService.cs
```

The SOLID principles from Laboratory Work 0 are preserved. Single Responsibility Principle keeps every class focused on one task. Open/Closed Principle allows new discounts to be added as new classes without changing existing code. Dependency Inversion Principle makes `TicketService` depend on the interfaces `INotifier` and `ITicketStorage` rather than on concrete classes.

## Singleton

This pattern guarantees that a class has exactly one instance in the entire program and provides a global access point to it. The constructor is declared `private`, so no code outside the class can call `new`. The only way to get the object is through the static property `Instance`, which creates the object on the first call and returns the same object on every subsequent call.

In this project the Singleton is `MuseumConfig`. It holds the museum name and the base ticket price. These values are needed in several places: `Program.cs` prints them at startup, and `PriceCalculator` reads the base price when calculating ticket costs. Because both places call `MuseumConfig.Instance`, they are guaranteed to read the same data from the same object.

```csharp
public class MuseumConfig
{
    private static MuseumConfig? _instance;

    public string MuseumName { get; } = "National Museum of Moldova";
    public decimal BasePrice { get; } = 100m;

    private MuseumConfig() { }

    public static MuseumConfig Instance => _instance ??= new MuseumConfig();
}
```

The operator `??=` means "if `_instance` is null, create a new object and assign it; otherwise do nothing". At the end of the program `ReferenceEquals(config, MuseumConfig.Instance)` returns `True`, confirming that both references point to the same object in memory.

## Prototype

This pattern allows an object to create a copy of itself. Instead of filling in all fields from scratch, the caller takes an existing object, calls `Clone()`, and receives a new object with the same data. After that the caller can change only the fields that need to differ, leaving the original untouched.

In this project the Prototype is the `Ticket` class. In Laboratory Work 0 it was a `record` (immutable), but now it is a `class` with setters so that the cloned copy can be modified after creation. The method `Clone()` creates a new `Ticket` with the same visitor name, discount name and price:

```csharp
public class Ticket
{
    public string Visitor { get; set; }
    public string Discount { get; set; }
    public decimal Price { get; set; }

    public Ticket(string visitor, string discount, decimal price)
    {
        Visitor = visitor;
        Discount = discount;
        Price = price;
    }

    public Ticket Clone() => new Ticket(Visitor, Discount, Price);
}
```

In `Program.cs` the first sold ticket (Ana, Standard, 100 MDL) is cloned, and the clone's visitor name is changed to "Ana (copy for archive)". The original ticket stays unchanged. `ReferenceEquals(original, clone)` returns `False`, proving that the clone is a separate object in memory, not a reference to the original.

## Abstract Factory

This pattern provides an interface for creating a family of related objects without specifying their concrete classes. Unlike Factory Method, which creates a single object, Abstract Factory creates a coordinated set of objects that belong together.

In this project the Abstract Factory is `ISeasonFactory`. It declares two methods: `CreateDiscount()` returns a discount, and `CreateGreeting()` returns a greeting message. Two concrete factories implement this interface:

```csharp
public interface ISeasonFactory
{
    IDiscount CreateDiscount();
    string CreateGreeting(string visitor);
}
```

`RegularSeasonFactory` creates `NoDiscount` (the price stays at 100 MDL) and a standard greeting "Welcome, Elena!". `NightMuseumFactory` creates `NightDiscount` (the price is multiplied by 0.6, giving a 40% discount) and a special greeting "Welcome to Museum Night, Petru! Special program tonight.".

The function `ServeVisitor` in `Program.cs` accepts a name and an `ISeasonFactory`. It calls `factory.CreateDiscount()` and `factory.CreateGreeting()` without knowing which season is active. The discount and the greeting are always consistent because they come from the same factory. To add a new season, for example "Open Day" with a 100% discount, one would create a new discount class in the `Pricing` folder and a new factory class in the `Factories` folder. No existing classes would need to change.

## Result

After running the program the following output is shown in the console:

```
Museum: National Museum of Moldova
Base price: 100 MDL

=== TICKET SALES ===
Ana: Standard, 100.00 MDL
Ion: Student -50%, 50.00 MDL
Maria: Senior -30%, 70.00 MDL

Total revenue: 220.00 MDL

=== PROTOTYPE ===
Original: Ana: Standard, 100.00 MDL
Clone:    Ana (copy for archive): Standard, 100.00 MDL
Same object? False

=== ABSTRACT FACTORY ===
Welcome, Elena!
  Standard: 100.00 MDL
Welcome to Museum Night, Petru! Special program tonight.
  Night -40%: 60.00 MDL

=== SINGLETON CHECK ===
Same instance? True
```

The first two lines come from the Singleton: the museum name and the base price are read from `MuseumConfig.Instance`. The ticket sales section shows the SOLID principles from Laboratory Work 0 still working: three tickets are sold with different discounts, and the total revenue is 220 MDL. The Prototype section shows that the clone has the same price and discount as the original but a different visitor name, and `ReferenceEquals` confirms it is a separate object. The Abstract Factory section shows two visitors served by two different factories: Elena gets the regular season (no discount, standard greeting) and Petru gets the museum night (40% discount, special greeting). The Singleton check at the end confirms that `MuseumConfig.Instance` returns the same object every time. On computers with Russian regional settings the decimal point may appear as a comma (100,00 instead of 100.00), but the values are the same.

## Conclusion

In this laboratory work the museum system grew from a single-file project into a structured C# solution with folders for abstractions, models, pricing, factories, infrastructure and services. Three creational design patterns were applied. Singleton ensures that the museum configuration exists in exactly one copy. Prototype allows tickets to be duplicated and modified without creating them from scratch. Abstract Factory produces coordinated sets of discounts and greetings for different museum seasons. The SOLID principles from Laboratory Work 0 remain in place, and the project is ready to grow further in the next laboratory works.
