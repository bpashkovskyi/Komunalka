namespace Accidents.Model;

public class Coordinates
{
    public double? OriginalLatitude { get; set; }
    public double? OriginalLongitude { get; set; }

    public double? ShiftedLatitude { get; set; }
    public double? ShiftedLongitude { get; set; }

    public double? ManuallyCorrectedLatitude { get; set; }
    public double? ManuallyCorrectedLongitude { get; set; }

    public double? Latitude => ManuallyCorrectedLatitude ?? ShiftedLatitude ?? OriginalLatitude;

    public double? Longitude => ManuallyCorrectedLongitude ?? ShiftedLongitude ?? OriginalLongitude;
}