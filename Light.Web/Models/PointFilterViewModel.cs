using Light.Model;

namespace Light.Web.Models;

public class PointFilterViewModel
{
    public PointFilterViewModel(List<Point> points)
    {
        Queues = points.Select(point => point.Queue.ToString()).ToFilter();
        Streets = points.Select(point => point.Address.Street).ToFilter();
    }

    public List<string> Queues { get; }

    public List<string> Streets { get; }
}

public static class FilterExtensions
{
    public static List<string> ToFilter(this IEnumerable<string> data)
    {
        return data.Distinct().Where(value => !string.IsNullOrWhiteSpace(value)).OrderBy(s => s).ToList();
    }
}