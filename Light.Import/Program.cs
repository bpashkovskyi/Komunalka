using ExcelDataReader;

using Komunalka.Persistence;

using Light.Model;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

namespace Light.Import;

internal class Program
{
    private static HttpClient _httpClient = new();

    static async Task Main(string[] args)
    {
        var connectionString = "Server=tcp:bpashkovskyi-malkos.database.windows.net,1433;Initial Catalog=bpashkovskyi-malkos-db;Persist Security Info=False;User ID=bpashkovskyi;Password=Q!W@E#r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<KomunalkaContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);


        for (int i = 0; i < 500; i++)
        {
            var accidentsContext = new KomunalkaContext(dbContextOptionsBuilder.Options);



            var toLocate = accidentsContext.Points.Where(p => p.Coordinates.OriginalLatitude == null).Take(1000).ToList();

            int j = 0;

            foreach (var point in toLocate)
            {
                await GeoCode(point).ConfigureAwait(false);
                Console.WriteLine(j);

                j++;
            }

            ////var queues = new List<string> { /*"1.1", "1.2", "2.1", "2.2", "3.1", "3.2", "4.1", "4.2", */"5.1", "5.2", "6.1", "6.2", "6.3", "6.4" };

            ////foreach (var queue in queues)
            ////{
            ////    var points = ParsePoints(queue);

            ////    ////foreach (var point in points)
            ////    ////{
            ////    ////    if (point.Address.City == "Івано-Франківськ")
            ////    ////    {
            ////    ////        await GeoCode(point);
            ////    ////    }
            ////    ////}

            ////    accidentsContext.Points.AddRange(points);
            ////}

            await accidentsContext.SaveChangesAsync().ConfigureAwait(false);
        }
        
    }

    static List<Point> ParsePoints(string queue)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var stream = File.Open($"D:\\{queue}.xlsx", FileMode.Open, FileAccess.Read);

        using var reader = ExcelReaderFactory.CreateReader(stream);

        var points = new List<Point>();
        do
        {
            while (reader.Read())
            {
                var region = reader.GetString(0);
                var city = reader.GetString(1);
                var street = reader.GetString(2);
                var numbers = reader.GetString(3).Split(", ");

                foreach (var number in numbers)
                {
                    var point = new Point
                    {
                        Queue = queue,
                        Address = new Address
                        {
                            Region = region,
                            City = city,
                            Street = street,
                            AdditionalAddress = number
                        }
                    };

                    points.Add(point);
                }
            }
        } while (reader.NextResult());

        return points;
    }

    private static async Task GeoCode(Point point)
    {
        try
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyD7PIVcFN5iX7CdxlyrU_NAisSI2RY9Lio&address={point.Address.City}, {point.Address.Street}, {point.Address.AdditionalAddress}";

            using HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jObject = JObject.Parse(jsonResponse);

            var locationObject = jObject["results"][0]["geometry"]["location"];

            point.Coordinates = new Coordinates
            {
                OriginalLatitude = locationObject["lat"].Value<double>(),
                OriginalLongitude = locationObject["lng"].Value<double>()
            };
        }
        catch (Exception ex)
        {
            point.Coordinates = new Coordinates
            {
                OriginalLatitude = -1,
                OriginalLongitude = -1
            };
        }
    }
}