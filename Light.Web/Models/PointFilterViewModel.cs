using Light.Model;

namespace Light.Web.Models;

public class PointFilterViewModel
{
    public PointFilterViewModel(List<Point> points)
    {
        Queues = points.Select(point => point.Queue.ToString()).ToFilter();
        Streets = points.Select(point => point.Address.Street).ToFilter();
    }

    public List<string> Queues { get; set; } = new();

    public List<string> Streets { get; set; } = new();
}

public static class FilterExtensions
{
    public static List<string> ToFilter(this IEnumerable<string> data)
    {
        return data.Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).ToList();
    }
}