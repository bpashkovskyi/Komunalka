using Accidents.Model;

namespace Accidents.Web.Models;

public class AccidentViewModel
{
    private const string Pedestrian =
        "https://mt.google.com/vt/icon/name=icons/onion/SHARED-mymaps-container-bg_4x.png,icons/onion/SHARED-mymaps-container_4x.png,icons/onion/1731-walking-pedestrian_4x.png";

    private const string Cyclist =
        "https://mt.google.com/vt/icon/name=icons/onion/SHARED-mymaps-container-bg_4x.png,icons/onion/SHARED-mymaps-container_4x.png,icons/onion/1522-bicycle_4x.png";

    private const string Car =
        "https://mt.google.com/vt/icon/name=icons/onion/SHARED-mymaps-container-bg_4x.png,icons/onion/SHARED-mymaps-container_4x.png,icons/onion/1538-car_4x.png";

    private const string Red = "highlight=ff000000,E65100,ff000000";
    private const string Yellow = "highlight=ff000000,F9A825,ff000000";
    private const string Green = "highlight=ff000000,0F9D58,ff000000";

    public AccidentViewModel(Accident accident)
    {
        Id = accident.Id;
        MarkerIcon = GetIcon(accident);
        Description = this.GetDescription(accident);
        Lat = accident.Coordinates?.Latitude ?? 0;
        Lng = accident.Coordinates?.Longitude ?? 0;
    }

    private string GetIcon(Accident accident)
    {
        var icon = Car;

        if (accident.CyclistInvolved)
        {
            icon = Cyclist;
        }

        if (accident.PedestrianInvolved)
        {
            icon = Pedestrian;
        }

        if (accident.Dead > 0)
        {
            icon = $"{icon}&{Red}";
        }
        else if (accident.MajorInjuries > 0)
        {
            icon = $"{icon}&{Yellow}";
        }
        else
        {
            icon = $"{icon}&{Green}";

        }

        return icon;
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

    public Guid Id { get; set; }
    public string MarkerIcon { get; set; }


    public string Description { get; set; }

    public double Lat { get; set; }
    public double Lng { get; set; }
}