using Museum.Abstractions;

namespace Museum.Services;

// Single Responsibility Principle (Lab 0): только считает цену
public class PriceCalculator
{
    public decimal Calculate(IDiscount discount) =>
        discount.Apply(MuseumConfig.Instance.BasePrice);
}
