namespace Museum.Abstractions;

// Abstract Factory Pattern: фабрика создаёт семейство связанных объектов (скидка + приветствие)
public interface ISeasonFactory
{
    IDiscount CreateDiscount();
    string CreateGreeting(string visitor);
}
