# Laboratory Work 1: SOLID Principles

## Topic and Goal

The topic of the semester project is a museum management system. In this first laboratory work the system is able to sell entrance tickets with different discounts. In the next laboratories it will grow with new features such as exhibits, exhibitions and tours. The goal of this work is to build the base of the project and to apply three principles from SOLID: Single Responsibility Principle, Open/Closed Principle and Dependency Inversion Principle.

## Project Overview

The whole program is written in one file, `Program.cs`, in C#. It sells three tickets to three visitors, each with a different discount, saves every ticket, shows a message to the visitor and calculates the total revenue. The base price of a ticket is 100 MDL. The program runs with the command `dotnet run`.

The work of selling one ticket goes through a simple chain. First the price is calculated by `PriceCalculator`, then the ticket is saved by `TicketList`, and finally a message is shown by `ConsoleNotifier`. The class `TicketService` runs this chain in the correct order.

## Single Responsibility Principle

This principle says that every class must have only one reason to change, which means only one job. In this project every class does exactly one thing. `PriceCalculator` only calculates the price, `TicketList` only stores tickets, `ConsoleNotifier` only shows messages, and `Ticket` only holds data. The class `TicketService` does not calculate, save or print anything by itself. It only calls the other classes one by one:

```csharp
public void Sell(string visitor, IDiscount discount)
{
    var price = _calculator.Calculate(discount);
    _storage.Save(new Ticket(visitor, discount.Name, price));
    _notifier.Notify($"{visitor}: {discount.Name}, {price:0.00} MDL");
}
```

Because of this separation, a change in one place does not affect the others. If the price formula changes, only `PriceCalculator` is edited. If messages must look different, only the notifier is edited. A good real-life example is a museum where the cashier counts money, the archivist keeps the journal, the postman sends letters and the administrator only gives tasks to them.

## Open/Closed Principle

This principle says that code must be open for extension but closed for modification. It means that new behavior should be added by writing new code, not by changing old code that already works. In the project this is done with the interface `IDiscount`. Each discount is a separate class that follows the same contract:

```csharp
interface IDiscount
{
    string Name { get; }
    decimal Apply(decimal price);
}

class StudentDiscount : IDiscount
{
    public string Name => "Student -50%";
    public decimal Apply(decimal price) => price * 0.5m;
}
```

The class `PriceCalculator` works with the interface and does not know which discount it receives. To add a new discount, for example for families, it is enough to write a new class `FamilyDiscount : IDiscount`. No existing class has to be changed. Without this approach the calculator would need a long chain of `if` conditions, and each new discount would require editing and possibly breaking the old code. A simple comparison is a power socket: a new device is just plugged in, and the wiring in the wall stays the same.

## Dependency Inversion Principle

This principle says that main classes must not depend on concrete details. Both of them must depend on abstractions. In the project the class `TicketService` does not use `ConsoleNotifier` or `TicketList` directly. It asks for the interfaces `INotifier` and `ITicketStorage` in its constructor:

```csharp
public TicketService(INotifier notifier, ITicketStorage storage)
{
    _notifier = notifier;
    _storage = storage;
}
```

The concrete classes are chosen in only one place, in the first line of the program:

```csharp
var service = new TicketService(new ConsoleNotifier(), new TicketList());
```

The service knows only what its helpers can do, not who they are. For this reason the console can be replaced with e-mail and the list in memory can be replaced with a database by writing two new classes and changing this single line. The code of `TicketService` stays the same. It is like a museum director who says "inform the visitor" and does not care if it is done by post, e-mail or phone.

## Result

After running the program the following output is shown in the console:

```
Ana: Standard, 100.00 MDL
Ion: Student -50%, 50.00 MDL
Maria: Senior -30%, 70.00 MDL

Total revenue: 220.00 MDL
```

The prices match the rules of the discounts: no discount gives 100 MDL, the student discount of 50% gives 50 MDL, and the senior discount of 30% gives 70 MDL. The total revenue is the sum of the three tickets, which is 220 MDL.

## Conclusion

In this laboratory work I created the base of a museum ticket system and applied three SOLID principles. Single Responsibility Principle made every class small and focused on one task. Open/Closed Principle made it possible to add new discounts without changing the existing code. Dependency Inversion Principle made the notification and storage parts replaceable. As a result the project is simple now, but it is ready to grow in the next laboratory works without being rewritten from the beginning.
