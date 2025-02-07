using System.Globalization;

using Accidents.Model;

using ExcelDataReader;

using Komunalka.Persistence;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

using Environment = Accidents.Model.Environment;

namespace Accidents.Import;

internal class Program
{
    private static readonly HttpClient HttpClient = new();

    static async Task Main()
    {
        var connectionString = "Server=tcp:bpashkovskyi-malkos.database.windows.net,1433;Initial Catalog=bpashkovskyi-malkos-db;Persist Security Info=False;User ID=bpashkovskyi;Password=Q!W@E#r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<KomunalkaContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);

        var accidents = ParseAccidents();
        foreach (var accident in accidents)
        {
            await GeoCode(accident);
        }

        ShiftCoordinates(accidents);

        var accidentsContext = new KomunalkaContext(dbContextOptionsBuilder.Options);
        accidentsContext.Accidents.AddRange(accidents);
        await accidentsContext.SaveChangesAsync();
    }

    private static void ShiftCoordinates(List<Accident> accidents)
    {
        var accidentsAtSamePoint = accidents.GroupBy(accident => new
        {
            accident.Coordinates.OriginalLatitude,
            accident.Coordinates.OriginalLongitude
        });

        foreach (var accidentGroup in accidentsAtSamePoint)
        {
            var shift = 0d;

            foreach (var accident in accidentGroup)
            {
                accident.Coordinates.ShiftedLatitude = accident.Coordinates.OriginalLatitude + shift;
                accident.Coordinates.ShiftedLongitude = accident.Coordinates.OriginalLongitude + shift;

                shift += 0.00001;
            }
        }
    }

    private static async Task GeoCode(Accident accident)
    {
        try
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyD7PIVcFN5iX7CdxlyrU_NAisSI2RY9Lio&address={accident.Address.City}, {accident.Address.Street}, {accident.Address.AdditionalAddress}";

            using HttpResponseMessage response = await HttpClient.GetAsync(url);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(jsonResponse);

            var locationObject = jObject["results"][0]["geometry"]["location"];

            accident.Coordinates = new Coordinates
            {
                OriginalLatitude = locationObject["lat"].Value<double>(),
                OriginalLongitude = locationObject["lng"].Value<double>()
            };
        }
        catch (Exception ex)
        {
            accident.Coordinates = new Coordinates
            {
                OriginalLatitude = null,
                OriginalLongitude = null
            };
        }
    }

    static List<Accident> ParseAccidents()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var stream = File.Open("D:\\accidents.xlsx", FileMode.Open, FileAccess.Read);

        using var reader = ExcelReaderFactory.CreateReader(stream);

        var accidents = new List<Accident>();

        do
        {
            while (reader.Read())
            {
                var dateTimeAsString = reader.GetString(0);

                if (!string.IsNullOrEmpty(dateTimeAsString))
                {
                    // this mean we have new accident

                    var accidentType = reader.GetString(1);
                    var originalEnvironment = reader.GetString(2);
                    var city = reader.GetString(3);
                    var street = reader.GetString(4);
                    var additionalAddress = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString();
                    var reason = reader.GetValue(6) == null ? null : reader.GetValue(6).ToString();
                    var casualty = reader.GetString(7);

                    var accident = new Accident
                    {

                        Type = accidentType.ToSentenceCase(),
                        Timestamp = DateTime.Parse(dateTimeAsString, new CultureInfo("uk-UA")),
                        Environment = new Environment(originalEnvironment),
                        Address = new Address
                        {
                            City = city.ToSentenceCase(),
                            Street = street.ToSentenceCase(),
                            AdditionalAddress = additionalAddress.ToSentenceCase()
                        }
                    };

                    accident.AddReason(reason.ToSentenceCase());
                    accident.Casualties.Add(casualty.ToSentenceCase());

                    accidents.Add(accident);
                }
                else
                {
                    // we have to add environment or casualty
                    var originalEnvironment = reader.GetString(2);
                    var casualty = reader.GetString(7);

                    if (!string.IsNullOrEmpty(originalEnvironment))
                    {
                        accidents.Last().Environment.Add(originalEnvironment.ToSentenceCase());
                    }

                    if (!string.IsNullOrEmpty(casualty))
                    {
                        accidents.Last().Casualties.Add(casualty.ToSentenceCase());
                    }
                }
            }
        } while (reader.NextResult());

        return accidents;
    }
}