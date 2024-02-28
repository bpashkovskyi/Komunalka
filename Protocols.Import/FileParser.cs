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

        foreach (var page in pdf.GetPages())
        {
            text += page.Text;
        }

        var firstPage = pdf.GetPages().FirstOrDefault();
        text += ContentOrderTextExtractor.GetText(firstPage);

        text = RemoveFooters(text);

        var itemRegex = new Regex("СЛУХАЛ?И?\\:?\\s*(.*?)\\s*\\d*\\.?ВИРІШИЛ?И?\\:?\\s*(.*?)\\s*\\.?\\s*\\d*\\.*\\s*?(?=СЛУХАЛ|Голов|Заступ)");


        var itemMatches = itemRegex.Matches(text);

        var items = new List<Item>();

        for (int i = 0; i < itemMatches.Count; i++)
        {
            var item = new Item
            {
                Number = i + 1,
                Heard = itemMatches[i].Groups[1].Value,
                Decided = itemMatches[i].Groups[2].Value
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