namespace Protocols.Web.Models;

public class ProtocolEditViewModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Date { get; set; }

    public List<ItemEditViewModel> Items { get; set; } = new();
}