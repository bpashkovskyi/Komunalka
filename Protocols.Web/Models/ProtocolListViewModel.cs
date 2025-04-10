using System.ComponentModel;
using System.Globalization;

using Protocols.Model;

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

    public static ProtocolListViewModel FromProtocol(Protocol protocol)
    {
        return new ProtocolListViewModel
        {
            Id = protocol.Id,
            Date = protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA")),
            Number = protocol.Number,
            ItemsCount = protocol.Items.Count,
        };
    }
}