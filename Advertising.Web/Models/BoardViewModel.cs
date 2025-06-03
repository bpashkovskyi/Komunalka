using Advertising.Model;

namespace Advertising.Web.Models;

public class BoardViewModel
{
    public BoardViewModel(Board board)
    {
        Id = board.Id;
        Description = this.GetDescription(board);
        Lat = board.Coordinates?.Latitude ?? 0;
        Lng = board.Coordinates?.Longitude ?? 0;
    }

    private string GetDescription(Board board)
    {
        var description = $"<b>Ід:</b> {board.Id}<br/>";
        description += $"<b>Розпосюджувач:</b> {board.NormalizedDistributorName}<br/>";
        description += $"<b>Ідентифікатор розповсюджувача:</b> {board.DistributorIdentifier}<br/>";
        description += $"<b>Тип:</b> {board.Type}<br/>";
        description += $"<b>Кількість рекламних площин:</b> {board.PlanesValue}<br/>";
        description += $"<b>Розмір по горизонталі:</b> {board.HorizontalSizeValue}<br/>";
        description += $"<b>Розмір по вертикалі:</b> {board.VerticalSizeValue}<br/>";
        description += $"<b>Площа:</b> {board.SquareValue}<br/>";
        description += $"<b>Адреса:</b> {board.Address.PostName} {board.Address.Thoroughfare} {board.Address.LocatorDesignator} {board.Address.Description}<br/>";

        return description;
    }

    public int Id { get; set; }

    public string Description { get; set; }

    public double Lat { get; set; }
    public double Lng { get; set; }
}