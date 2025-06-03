using System.ComponentModel;

namespace Protocols.Web.Models;

public class ItemEditViewModel
{
    public int? Id { get; set; }
    public int OrderNumber { get; set; }

    [DisplayName("Номер")]
    public int? Number { get; set; }

    [DisplayName("Слухали")]
    public string Heard { get; set; }

    [DisplayName("Вирішили")]
    public string Decided { get; set; }
}