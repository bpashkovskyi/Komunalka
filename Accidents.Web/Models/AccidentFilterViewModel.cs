using Accidents.Model;

namespace Accidents.Web.Models;

public class AccidentFilterViewModel
{
    public AccidentFilterViewModel(List<Accident> accidents)
    {
        Cities = accidents.Select(accident => accident.Address.City.ToString()).ToFilter();
        Years = accidents.Select(accident => accident.Timestamp.Year.ToString()).ToFilter();
        Types = accidents.Select(accident => accident.Type).ToFilter();
        Reasons = accidents.SelectMany(accident => accident.Reasons.Select(reason => reason)).ToFilter();

        SurfaceTypes = accidents.Select(accident => accident.Environment.SurfaceType).ToFilter();
        SurfaceConditions = accidents.Select(accident => accident.Environment.SurfaceCondition).ToFilter();
        Lightings = accidents.Select(accident => accident.Environment.Lighting).ToFilter();
        RoadElements = accidents.Select(accident => accident.Environment.RoadElements).ToFilter();
        Constructions = accidents.Select(accident => accident.Environment.Constructions).ToFilter();
        WeatherConditions = accidents.Select(accident => accident.Environment.Weather).ToFilter();
        TrafficTools = accidents.SelectMany(accident => accident.Environment.TrafficTools.Select(trafficTool => trafficTool)).ToFilter();
    }

    public List<string> Cities { get; init; }

    public List<string> Years { get; init; }

    public List<string> Types { get; init; }

    public List<string> Reasons { get; init; }

    public List<string> SurfaceTypes { get; init; }

    public List<string> SurfaceConditions { get; init; }

    public List<string> Lightings { get; init; }

    public List<string> RoadElements { get; init; }

    public List<string> Constructions { get; init; }

    public List<string> WeatherConditions { get; init; }

    public List<string> TrafficTools { get; init; }
}

public static class FilterExtensions
{
    public static List<string> ToFilter(this IEnumerable<string> data)
    {
        return data.Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).ToList();
    }
}