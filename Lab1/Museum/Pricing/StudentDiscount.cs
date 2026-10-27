using Museum.Abstractions;

namespace Museum.Pricing;

public class StudentDiscount : IDiscount
{
    public string Name => "Student -50%";
    public decimal Apply(decimal price) => price * 0.5m;
}
