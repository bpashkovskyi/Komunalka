using System.ComponentModel;
using System.Globalization;
using Protocols.Model;

namespace Protocols.Web.Models;

public class ItemSearchViewModel
{
    public int ProtocolId { get; set; }

    [DisplayName("Протокол №")]
    public string ProtocolNumber { get; set; }

    [DisplayName("Дата")]
    public string ProtocolDate { get; set; }

    [DisplayName("Номер")]
    public int? Number { get; set; }

    [DisplayName("Слухали")]
    public string Heard { get; set; }

    [DisplayName("Вирішили")]
    public string Decided { get; set; }

    public ItemSearchViewModel(Item item)
    {
        ProtocolId = item.Protocol.Id;
        ProtocolNumber = item.Protocol.Number;
        ProtocolDate = item.Protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA"));
        Number = item.Number;
        Heard = item.Heard;
        Decided = item.Decided;
    }
}