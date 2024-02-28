using System.ComponentModel;

namespace Protocols.Web.Models;

public class ProtocolListViewModel
{
    public int Id { get; set; }

    [DisplayName("Протокол №")]
    public string Number { get; set; }

    [DisplayName("Дата")]
    public string Date { get; set; }

    [DisplayName("Кількість питань")]
    public int ItemsCount { get; set; }
}