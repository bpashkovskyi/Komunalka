using Advertising.Model;

namespace Advertising.Web.Models;

public class BoardSearchViewModel
{
    public List<string> DistributorNames { get; init; } = new();

    public List<string> Statuses { get; init; } = new();

    public List<string> Types { get; init; } = new();

    public bool? IsCoordinatesChecked { get; set; }

    public List<Board> Filter(List<Board> boards)
    {
        if (DistributorNames.Any())
        {
            boards = boards.Where(board => DistributorNames.Contains(board.NormalizedDistributorName)).ToList();
        }

        if (Statuses.Any())
        {
            boards = boards.Where(board => Statuses.Contains(board.Status)).ToList();
        }

        if (Types.Any())
        {
            boards = boards.Where(board => Types.Contains(board.NormalizedType)).ToList();
        }


        if (IsCoordinatesChecked.HasValue)
        {
            if (IsCoordinatesChecked == true)
            {
                boards = boards.Where(accident => accident.Coordinates is { ManuallyCorrectedLatitude: not null, ManuallyCorrectedLongitude: not null }).ToList();
            }
            else
            {
                boards = boards.Where(accident => accident.Coordinates?.ManuallyCorrectedLatitude == null || accident.Coordinates?.ManuallyCorrectedLongitude == null).ToList();
            }
        }

        return boards;
    }

}