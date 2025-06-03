namespace Protocols.Model;

public class Item
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public int OrderNumber { get; set; }

    public string Heard { get; set; }

    public string Decided { get; set; }

    public Protocol Protocol { get; set; }

    public int ProtocolId { get; set; }
}