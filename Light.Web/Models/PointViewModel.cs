
using Light.Model;

namespace Light.Web.Models;

public class PointViewModel
{
    public PointViewModel(Point point)
    {
        Id = point.Id;
        MarkerColor = GetColor(point);
        Lat = point.Coordinates?.OriginalLatitude ?? 0;
        Lng = point.Coordinates?.OriginalLongitude ?? 0;
        Description = $"{point.Queue}. {point.Address.Street} {point.Address.AdditionalAddress}";
    }

    private static string GetColor(Point point)
    {
        var color = point.Queue switch
        {
            "1.1" => "red",
            "1.2" => "red-dot",
            "2.1" => "orange",
            "2.2" => "orange-dot",
            "3.1" => "yellow",
            "3.2" => "yellow-dot",
            "4.1" => "green",
            "4.2" => "green-dot",
            "5.1" => "blue",
            "5.2" => "blue-dot",
            "6.1" => "pink",
            "6.2" => "pink-dot",
            "6.3" => "red",
            "6.4" => "red-dot"
        };

        return color;
    }

    public Guid Id { get; set; }
    public string MarkerColor { get; set; }


    public double Lat { get; set; }
    public double Lng { get; set; }

    public string Description { get; set; }
}