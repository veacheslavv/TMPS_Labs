using Museum.Abstractions;

namespace Museum.Pricing;

public class NightDiscount : IDiscount
{
    public string Name => "Night -40%";
    public decimal Apply(decimal price) => price * 0.6m;
}
