namespace Museum.Models;

// Prototype Pattern: объект умеет клонировать сам себя через Clone()
public class Ticket
{
    public string Visitor { get; set; }
    public string Discount { get; set; }
    public decimal Price { get; set; }

    public Ticket(string visitor, string discount, decimal price)
    {
        Visitor = visitor;
        Discount = discount;
        Price = price;
    }

    public Ticket Clone() => new Ticket(Visitor, Discount, Price);

    public override string ToString() => $"{Visitor}: {Discount}, {Price:0.00} MDL";
}
