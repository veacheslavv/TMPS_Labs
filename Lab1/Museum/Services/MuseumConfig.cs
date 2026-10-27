namespace Museum.Services;

// Singleton Pattern: приватный конструктор, доступ только через Instance
// Во всей программе существует ровно один экземпляр этого класса
public class MuseumConfig
{
    private static MuseumConfig? _instance;

    public string MuseumName { get; } = "National Museum of Moldova";
    public decimal BasePrice { get; } = 100m;

    private MuseumConfig() { }

    public static MuseumConfig Instance => _instance ??= new MuseumConfig();
}
