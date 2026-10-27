using Museum.Abstractions;

namespace Museum.Pricing;

public class NoDiscount : IDiscount
{
    public string Name => "Standard";
    public decimal Apply(decimal price) => price;
}
