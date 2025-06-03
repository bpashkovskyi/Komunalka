using Advertising.Model;

namespace Advertising.Web.Models;

public class BoardFilterViewModel
{
    public BoardFilterViewModel(List<Board> boards)
    {
        DistributorNames = boards.Select(board => board.NormalizedDistributorName).ToFilter();
        Statuses = boards.Select(board => board.Status).ToFilter();
        Types = boards.Select(board => board.NormalizedType).ToFilter();
    }

    public List<string> DistributorNames { get; init; }

    public List<string> Statuses { get; init; }

    public List<string> Types { get; init; }
}

public static class FilterExtensions
{
    public static List<string> ToFilter(this IEnumerable<string> data)
    {
        return data.Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).ToList();
    }
}