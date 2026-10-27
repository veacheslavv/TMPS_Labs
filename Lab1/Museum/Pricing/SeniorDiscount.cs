using Museum.Abstractions;

namespace Museum.Pricing;

public class SeniorDiscount : IDiscount
{
    public string Name => "Senior -30%";
    public decimal Apply(decimal price) => price * 0.7m;
}
