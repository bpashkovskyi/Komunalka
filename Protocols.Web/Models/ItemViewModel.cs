using System.ComponentModel;

namespace Protocols.Web.Models;

public class ItemViewModel
{
    [DisplayName("Номер")]
    public int? Number { get; set; }

    [DisplayName("Слухали")]
    public string Heard { get; set; }

    [DisplayName("Вирішили")]
    public string Decided { get; set; }
}