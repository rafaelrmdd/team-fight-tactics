namespace backend.Models;

public class Piece
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public decimal Price { get; set; }

    public Piece(Guid id, string name, string role, decimal price)
    {
        Id = id;
        Name = name;
        Role = role;
        Price = price;
    }
}