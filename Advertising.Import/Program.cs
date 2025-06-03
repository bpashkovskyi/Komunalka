using System.Globalization;
using System.Text.RegularExpressions;

using Advertising.Model;

using ExcelDataReader;

using Komunalka.Persistence;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

namespace Advertising.Import;

internal class Program
{
    private static readonly HttpClient HttpClient = new();

    static async Task Main()
    {
        var connectionString = "Server=tcp:bpashkovskyi-malkos.database.windows.net,1433;Initial Catalog=bpashkovskyi-malkos-db;Persist Security Info=False;User ID=bpashkovskyi;Password=Q!W@E#r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<KomunalkaContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);

        var boardsContext = new KomunalkaContext(dbContextOptionsBuilder.Options);

        var boards = ParseBoards();
        foreach (var board in boards)
        {
            await GeoCode(board).ConfigureAwait(false);
        }

        ShiftCoordinates(boards);

        ////var boards = await boardsContext.Boards.ToListAsync();

        foreach (var board in boards)
        {
            Normalize(board, boards);
        }

        ////foreach (var board in boards)
        ////{
        ////    await GeoCode(board).ConfigureAwait(false);
        ////}

        ////ShiftCoordinates(boards);

        boardsContext.Boards.AddRange(boards);
        await boardsContext.SaveChangesAsync().ConfigureAwait(false);

    }

    private static void ShiftCoordinates(List<Board> boards)
    {
        var boardsAtSamePoint = boards.GroupBy(board => new
        {
            board.Coordinates.OriginalLatitude,
            board.Coordinates.OriginalLongitude
        });

        foreach (var boardGroup in boardsAtSamePoint)
        {
            var shift = 0d;

            foreach (var board in boardGroup)
            {
                board.Coordinates.ShiftedLatitude = board.Coordinates.OriginalLatitude + shift;
                board.Coordinates.ShiftedLongitude = board.Coordinates.OriginalLongitude + shift;

                shift += 0.00001;
            }
        }
    }

    private static async Task GeoCode(Board board)
    {
        try
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyD7PIVcFN5iX7CdxlyrU_NAisSI2RY9Lio&address={board.Address.PostName}, {board.Address.Thoroughfare}, {board.Address.LocatorDesignator}";

            using HttpResponseMessage response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jObject = JObject.Parse(jsonResponse);

            var locationObject = jObject["results"][0]["geometry"]["location"];

            board.Coordinates = new Coordinates
            {
                OriginalLatitude = locationObject["lat"].Value<double>(),
                OriginalLongitude = locationObject["lng"].Value<double>()
            };
        }
        catch (Exception ex)
        {
            board.Coordinates = new Coordinates
            {
                OriginalLatitude = null,
                OriginalLongitude = null
            };
        }
    }

    private static List<Board> ParseBoards()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var boards = new List<Board>();

        using var stream = File.Open("D:\\maf.xlsx", FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var result = reader.AsDataSet();

        var table = result.Tables[0];

        // Skip first two rows (assume first is column header, second is blank or formatting row)
        for (int row = 3; row < table.Rows.Count; row++)
        {
            var dataRow = table.Rows[row];
            if (dataRow == null || dataRow.ItemArray.All(field => string.IsNullOrWhiteSpace(field?.ToString()))) continue;

            var record = new Board
            {
                Identifier = ParseString(dataRow[0]),
                Issued = ParseDate(dataRow[1]),
                AuthorityName = ParseString(dataRow[2]),
                AuthorityIdentifier = ParseString(dataRow[3]),
                DistributorName = ParseString(dataRow[4]),
                DistributorIdentifier = ParseString(dataRow[5]),
                Status = ParseString(dataRow[6]),
                ValidFrom = ParseDate(dataRow[7]),
                ValidThrough = ParseDate(dataRow[8]),
                PlanesValue = ParseDouble(dataRow[9]),
                SquareValue = ParseDouble(dataRow[10]),
                Type = "Маф",
                Address = new Address
                {
                    PostCode = ParseString(dataRow[11]),
                    AdminUnitL1 = ParseString(dataRow[12]),
                    AdminUnitL2 = ParseString(dataRow[13]),
                    AdminUnitL3 = ParseString(dataRow[14]),
                    PostName = ParseString(dataRow[15]),
                    Thoroughfare = ParseString(dataRow[16]),
                    LocatorDesignator = ParseString(dataRow[17]),
                    LocatorBuilding = ParseString(dataRow[18]),
                }
            };

            boards.Add(record);
        }

        return boards;
    }

    private static string ParseString(object val)
    {
        var stringValue = val?.ToString();

        if (stringValue == "null" || string.IsNullOrEmpty(stringValue))
        {
            return null;
        }

        return stringValue.Trim();
    }

    private static DateTime? ParseDate(object val)
    {
        if (DateTime.TryParse(val?.ToString(), out var date))
            return date;
        return null;
    }

    private static double? ParseDouble(object val)
    {
        if (double.TryParse(val?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var num))
            return num;
        return null;
    }

    private static void Normalize(Board board, List<Board> allBoards)
    {
        board.NormalizedType = Classify(board.Type);
        board.NormalizedDistributorName = allBoards.FirstOrDefault(a => a.DistributorIdentifier != null && a.DistributorIdentifier == board.DistributorIdentifier)?.DistributorName ?? board.DistributorName;
    }

    static string Classify(string input)
    {
        if (input == null)
        {
            return "інші";
        }

        if (input.Contains("сіті-лайт"))
            return "сіті-лайт";
        if (input.Contains("об’ємні") && input.Contains("літери"))
            return "об’ємні літери";
        if (input.Contains("банер"))
            return "банер";
        if (input.Contains("білборд") || input.Contains("біл-борд"))
            return "білборд";
        if (input.Contains("брандмауер") || input.Contains("брендмауер"))
            return "брандмауер";
        if (input.Contains("стела"))
            return "стела";
        if (input.Contains("вказівник"))
            return "вказівник";
        if (input.Contains("кронштейн"))
            return "кронштейн";
        if (input.Contains("настінна конструкція"))
            return "настінна конструкція";
        if (input.Contains("призматрон"))
            return "призматрон";
        if (input.Contains("рекламна дахова конструкція"))
            return "рекламна дахова конструкція";
        if (input.Contains("світлодіодний екран"))
            return "світлодіодний екран";
        if (input.Contains("світлодіодна стрічка"))
            return "світлодіодна стрічка";
        if (input.Contains("світлодіодне табло"))
            return "світлодіодне табло";
        if (input.Contains("щит"))
            return "щит";
        if (input.Contains("телевізійний екран"))
            return "телевізійний екран";
        return "інші";
    }
}