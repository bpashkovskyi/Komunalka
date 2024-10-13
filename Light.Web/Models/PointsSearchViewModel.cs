using Light.Model;

namespace Light.Web.Models;

public class PointsSearchViewModel
{
    public List<string> Queues { get; set; } = new();

    public List<string> Streets { get; set; } = new();

    public List<Point> Filter(List<Point> points)
    {
        if (Queues.Any())
        {
            points = points.Where(point => Queues.Contains(point.Queue)).ToList();
        }

        if (Streets.Any())
        {
            points = points.Where(point => Streets.Contains(point.Address.Street)).ToList();
        }

        return points;
    }

}