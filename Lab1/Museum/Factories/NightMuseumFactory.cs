using Museum.Abstractions;
using Museum.Pricing;

namespace Museum.Factories;

public class NightMuseumFactory : ISeasonFactory
{
    public IDiscount CreateDiscount() => new NightDiscount();
    public string CreateGreeting(string visitor) =>
        $"Welcome to Museum Night, {visitor}! Special program tonight.";
}
