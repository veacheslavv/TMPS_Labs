namespace Museum.Abstractions;

// Open/Closed Principle (Lab 0): новая скидка = новый класс, старый код не меняется
public interface IDiscount
{
    string Name { get; }
    decimal Apply(decimal price);
}
