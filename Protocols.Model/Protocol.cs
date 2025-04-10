using System.Globalization;

namespace Protocols.Model;

public class Protocol
{
    public int Id { get; set; }

    public string Number { get; set; }

    public DateTime Date { get; set; }

    public List<Item> Items = new();

    public string FileName => $"Протокол №{this.Number} від {this.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA"))} року.pdf";
}