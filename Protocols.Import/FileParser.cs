using System.Globalization;
using System.Text.RegularExpressions;

using Protocols.Model;

using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Protocols.Import;

public class FileParser
{
    public static Protocol GetProtocolNumberAndDate(string filePath)
    {
        using var pdf = PdfDocument.Open(filePath);
        var firstPage = pdf.GetPages().FirstOrDefault();
        var text = ContentOrderTextExtractor.GetText(firstPage);

        var regex = new Regex("ПРОТОКОЛ \u2116\\s*(\\d+)\\s*від (\\d+\\.\\d+.\\d+)\\s*року");

        var match = regex.Match(text);

        if (!match.Success)
        {
            Console.WriteLine($"Protocol number is not found in file {filePath}");
            return null;
        }

        var protocol = new Protocol
        {
            Number = match.Groups[1].Value,
            Date = DateTime.Parse(match.Groups[2].Value, new CultureInfo("uk-UA")),
        };

        return protocol;
    }

    public static List<Item> GetProtocolItems(string filePath)
    {
        using var pdf = PdfDocument.Open(filePath);
        var text = string.Empty;

        var pages = pdf.GetPages().ToList();
        foreach (var page in pages)
        {
            text += page.Text;
        }

        var firstPage = pages.FirstOrDefault();
        text += ContentOrderTextExtractor.GetText(firstPage);

        text = RemoveFooters(text);

        var itemRegex = new Regex("(\\d*)\\.*\\s*СЛУХАЛ?И?\\:?\\s*(.*?)\\s*\\d*\\.?ВИРІШИЛ?И?\\:?\\s*(.*?)\\s*\\.?\\s*\\d*\\.*\\s*?[^\\d](?=\\d+\\.*\\s*СЛУХАЛ|Голов|Заступ)");


        var itemMatches = itemRegex.Matches(text);

        var items = new List<Item>();

        for (int i = 0; i < itemMatches.Count; i++)
        {
            var item = new Item
            {
                Number = itemMatches[i].Groups[1].Value == "" ? null : int.Parse(itemMatches[i].Groups[1].Value),
                OrderNumber = (i + 1) * 10,
                Heard = itemMatches[i].Groups[2].Value,
                Decided = itemMatches[i].Groups[3].Value
            };

            items.Add(item);

        }

        return items;
    }

    private static string RemoveFooters(string text)
    {
        var footerRegex = new Regex("(?:_+|Виконавчий) .*? \\d+ з \\d+");

        var matches = footerRegex.Matches(text);

        foreach (Match match in matches)
        {
            text = text.Replace(match.Value, string.Empty);
        }

        return text;
    }
}