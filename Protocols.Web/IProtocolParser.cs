using Protocols.Model;

using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Protocols.Web
{
    public interface IProtocolParser
    {
        Protocol Parse(Stream fileStream, out List<string> validationErrors);
    }

    public class ProtocolParser : IProtocolParser
    {
        public Protocol Parse(Stream fileStream, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            try
            {   using var pdf = PdfDocument.Open(fileStream);

                var protocol = new Protocol
                {
                    Items = GetProtocolItems(pdf, validationErrors)
                };

                return protocol; 
            }
            catch(Exception ex)
            {
                validationErrors.Add($"Трапилася нередбачувана помилка: {ex.Message}");
                return null;
            }
        }

        private static List<Item> GetProtocolItems(PdfDocument pdf, List<string> validationErrors)
        {
            var text = string.Empty;

            var pages = pdf.GetPages().ToList();
            foreach (var page in pages)
            {
                text += page.Text;
            }

            var firstPage = pages.FirstOrDefault();
            text += ContentOrderTextExtractor.GetText(firstPage);

            text = RemoveFooters(text, validationErrors);

            var heardRegex = new Regex("СЛУХАЛИ\\:", RegexOptions.IgnoreCase);
            var decidedRegex = new Regex("ВИРІШИЛИ\\:", RegexOptions.IgnoreCase);

            ////if (heardRegex.Matches(text).Count != decidedRegex.Matches(text).Count)
            ////{
            ////    validationErrors.Add($"У протоколі кількість вживань слова 'СЛУХАЛИ:' - {heardRegex.Matches(text).Count}, слова 'ВИРІШИЛИ:' - {decidedRegex.Matches(text).Count}");
            ////}

            var itemRegex = new Regex("(\\d*)\\.*\\s*СЛУХАЛИ\\:?\\s*(.*?)\\s*\\d*\\.?ВИРІШИЛИ\\:?\\s*(.*?)\\s*\\.?\\s*\\d*\\.*\\s*?[^\\d](?=\\d+\\.*\\s*СЛУХАЛ|Голов|Заступ)", RegexOptions.IgnoreCase);

            var itemMatches = itemRegex.Matches(text);
            if (itemMatches.Count == 0)
            {
                validationErrors.Add($"У протоколі не знайдено питань, переконайтеся, що він містить слова: 'СЛУХАЛИ' та 'ВИРІШИЛИ'");

            }

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

        private static string RemoveFooters(string text, List<string> validationErrors)
        {
            var footerRegex = new Regex("(?:_+|Виконавчий) .*? (Сторінка\\s*\\d+\\s*з\\s*\\d+|Голова|Оригінал)", RegexOptions.IgnoreCase);

            var matches = footerRegex.Matches(text);

            foreach (Match match in matches)
            {
                var footerLength = match.Value.Length;

                if (footerLength > 1000)
                {
                    validationErrors.Add("Занадто великий футер, зверніться до розробника");
                }

                text = text.Replace(match.Value, string.Empty);
            }

            return text;
        }
    }

    
    }
