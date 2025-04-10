using Protocols.Model;
using System.Globalization;

namespace Protocols.Web.Models;

public class ProtocolDetailsViewModel
{
    public int Id { get; set; }

    public string Number { get; set; }

    public string Date { get; set; }

    public List<ItemViewModel> Items { get; set; }

    public ProtocolDetailsViewModel (Protocol protocol)
    {
        Id = protocol.Id;
        Date = protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA"));
        Number = protocol.Number;
        Items = protocol.Items.OrderBy(item => item.OrderNumber).Select(item => new ItemViewModel(item)).ToList();
    }
}