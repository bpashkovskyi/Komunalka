namespace Protocols.Model;

public class Protocol
{
    public int Id { get; set; }

    public string Number { get; set; }

    public DateTime Date { get; set; }

    public List<Item> Items = new();
}