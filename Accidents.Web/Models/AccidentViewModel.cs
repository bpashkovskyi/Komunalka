using Accidents.Model;

namespace Accidents.Web.Models;

public class AccidentViewModel
{
    public AccidentViewModel(Accident accident)
    {
        Id = accident.Id;
        MarkerColor = GetColor(accident);
        Description = this.GetDescription(accident);
        Lat = accident.Coordinates?.Latitude ?? 0;
        Lng = accident.Coordinates?.Longitude ?? 0;
    }

    private string GetDescription(Accident accident)
    {
        var description = $"<b>Ід:</b> {accident.Id}<br/>";
        description += $"<b>Дата:</b> {accident.Timestamp}<br/>";
        description += $"<b>Адреса:</b> {accident.Address.City} {accident.Address.Street} {accident.Address.AdditionalAddress}<br/>";
        description += $"<b>Вид пригоди:</b> {accident.Type }<br/>";
        description += $"<b>Основна причина ДТП:</b> {string.Join(',', accident.Reasons.Select(reason => reason))}<br/>";
        description += $"<b>Тип покриття:</b> {accident.Environment.SurfaceType}<br/>";
        description += $"<b>Стан покриття:</b> {accident.Environment.SurfaceCondition}<br/>";
        description += $"<b>Освітленість:</b> {accident.Environment.Lighting}<br/>";
        description += $"<b>Елементи ділянки:</b> {accident.Environment.RoadElements}<br/>";
        description += $"<b>Штучні споруди:</b> {accident.Environment.Constructions}<br/>";
        description += $"<b>Погодні умови:</b> {accident.Environment.Weather}<br/>";
        description += $"<b>Технічні засоби організації дорожнього руху:</b> {string.Join(',', accident.Environment.TrafficTools)}<br/>";
        description += $"<b>Є місцем концентрації ДТП:</b> {accident.Environment.IsPlaceOfAccidentConcentration}<br/>";
        description += $"<b>Постраждалі:</b>Легко травмованих: {accident.MinorInjuries}. Важко травмованих: {accident.MajorInjuries}. Загиблих: {accident.Dead}.";

        return description;
    }

    private static string GetColor(Accident accident)
    {
        var color = "green";

        if (accident.Dead > 0)
        {
            color = "red";
        }
        else if (accident.MajorInjuries > 0)
        {
            color = "yellow";
        }

        if (accident.CyclistInvolved || accident.PedestrianInvolved)
        {
            color += "-dot";
        }

        return color;
    }

    public Guid Id { get; set; }
    public string MarkerColor { get; set; }

    public string Description { get; set; }

    public double Lat { get; set; }
    public double Lng { get; set; }
}