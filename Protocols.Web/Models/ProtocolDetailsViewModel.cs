namespace Protocols.Web.Models;

public class ProtocolDetailsViewModel
{
    public int Id { get; set; }

    public string Number { get; set; }

    public string Date { get; set; }

    public List<ItemViewModel> Items { get; set; }
}