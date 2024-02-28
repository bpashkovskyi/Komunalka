using Accidents.Model;

namespace Accidents.Web.Models;

public class AccidentSearchViewModel
{
    public bool? IsPedestrianInvolved { get; set; }

    public bool? IsCyclistInvolved { get; set; }

    public List<string> Cities { get; set; } = new();

    public List<string> Years { get; set; } = new();

    public List<string> Types { get; set; } = new();

    public List<string> Reasons { get; set; } = new();

    public List<string> SurfaceTypes { get; set; } = new();

    public List<string> SurfaceConditions { get; set; } = new();

    public List<string> Lightings { get; set; } = new();

    public List<string> RoadElements { get; set; } = new();

    public List<string> Constructions { get; set; } = new();

    public List<string> WeatherConditions { get; set; } = new();

    public List<string> TrafficTools { get; set; } = new();

    public string Casualties { get; set; }

    public bool? IsPlaceOfAccidentConcentration { get; set; }

    public bool? IsCoordinatesChecked { get; set; }

    public List<Accident> Filter(List<Accident> accidents)
    {
        if (IsPedestrianInvolved.HasValue)
        {
            accidents = accidents.Where(accident => accident.PedestrianInvolved == IsPedestrianInvolved).ToList();
        }

        if (IsCyclistInvolved.HasValue)
        {
            accidents = accidents.Where(accident => accident.CyclistInvolved == IsCyclistInvolved).ToList();
        }

        if (Cities.Any())
        {
            accidents = accidents.Where(accident => Cities.Contains(accident.Address.City)).ToList();
        }

        if (Years.Any())
        {
            accidents = accidents.Where(accident => Years.Contains(accident.Timestamp.Year.ToString())).ToList();
        }

        if (Types.Any())
        {
            accidents = accidents.Where(accident => Types.Contains(accident.Type)).ToList();
        }

        if (Reasons.Any())
        {
            accidents = accidents.Where(accident => Reasons.Any(searchReason => accident.Reasons.Any(reason => reason == searchReason) )).ToList();
        }

        if (SurfaceTypes.Any())
        {
            accidents = accidents.Where(accident => SurfaceTypes.Contains(accident.Environment.SurfaceType)).ToList();
        }

        if (SurfaceConditions.Any())
        {
            accidents = accidents.Where(accident => SurfaceConditions.Contains(accident.Environment.SurfaceCondition)).ToList();
        }

        if (Lightings.Any())
        {
            accidents = accidents.Where(accident => Lightings.Contains(accident.Environment.Lighting)).ToList();
        }

        if (RoadElements.Any())
        {
            accidents = accidents.Where(accident => RoadElements.Contains(accident.Environment.RoadElements)).ToList();
        }

        if (Constructions.Any())
        {
            accidents = accidents.Where(accident => Constructions.Contains(accident.Environment.Constructions)).ToList();
        }

        if (WeatherConditions.Any())
        {
            accidents = accidents.Where(accident => WeatherConditions.Contains(accident.Environment.Weather)).ToList();
        }

        if (TrafficTools.Any())
        {
            accidents = accidents.Where(accident => TrafficTools.Any(searchTrafficTool => accident.Environment.TrafficTools.Any(trafficTool => trafficTool == searchTrafficTool))).ToList();
        }

        accidents = Casualties switch
        {
            "dead" => accidents.Where(accident => accident.Dead > 0).ToList(),
            "major" => accidents.Where(accident => accident.Dead == 0 && accident.MajorInjuries > 0).ToList(),
            "minor" => accidents.Where(accident => accident.Dead == 0 && accident.MajorInjuries == 0 && accident.MinorInjuries > 0).ToList(),
            _ => accidents
        };

        if (IsPlaceOfAccidentConcentration.HasValue)
        {
            accidents = accidents.Where(accident => accident.Environment.IsPlaceOfAccidentConcentration == IsPlaceOfAccidentConcentration).ToList();
        }

        if (IsCoordinatesChecked.HasValue)
        {
            if (IsCoordinatesChecked == true)
            {
                accidents = accidents.Where(accident => accident.Coordinates?.ManuallyCorrectedLatitude != null && accident.Coordinates?.ManuallyCorrectedLongitude != null).ToList();
            }
            else
            {
                accidents = accidents.Where(accident => accident.Coordinates?.ManuallyCorrectedLatitude == null || accident.Coordinates?.ManuallyCorrectedLongitude == null).ToList();
            }
        }

        return accidents;
    }

}