using Museum.Abstractions;
using Museum.Pricing;

namespace Museum.Factories;

public class RegularSeasonFactory : ISeasonFactory
{
    public IDiscount CreateDiscount() => new NoDiscount();
    public string CreateGreeting(string visitor) => $"Welcome, {visitor}!";
}
