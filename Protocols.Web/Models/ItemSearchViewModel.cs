using System.ComponentModel;

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
}