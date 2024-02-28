namespace Accidents.Model;

public class Environment
{
    private Environment()
    {
    }

    public Environment(string rawData)
    {
        this.Add(rawData);
    }

    public void Add(string rawData)
    {
        if (rawData == null)
        {
            return;
        }

        var indexOfSemiColon = rawData.IndexOf(":");

        if (indexOfSemiColon == -1)
        {
            IsPlaceOfAccidentConcentration = rawData switch
            {
                "НЕ Є МІСЦЕМ КОНЦЕНТРАЦІЇ ДТП" => false,
                "Є МІСЦЕМ КОНЦЕНТРАЦІЇ ДТП" => true,
                _ => IsPlaceOfAccidentConcentration
            };

            return;
        }

        var key = rawData.Substring(0, indexOfSemiColon);

        var valueStartIndex = rawData[indexOfSemiColon + 1] != ' ' ? indexOfSemiColon + 1 : indexOfSemiColon + 2;
        var value = rawData.Substring(valueStartIndex, rawData.Length - valueStartIndex).ToSentenceCase();

        switch (key)
        {
            case "Тип покриття":
                SurfaceType = value;
                break;
            case "СТАН ПОКРИТТЯ":
                SurfaceCondition = value;
                break;
            case "ОСВІТЛЕНІСТЬ":
                Lighting = value;
                break;
            case "ЕЛЕМЕНТИ ДІЛЯНКИ":
                RoadElements = value;
                break;
            case "ШТУЧНІ СПОРУДИ":
                Constructions = value;
                break;
            case "ПОГОДНІ УМОВИ":
                Weather = value;
                break;
            case "ТЕХ.ЗАСОБИ ОРГ. ДОР. РУХУ":
                TrafficTools.Add(value); ;
                break;
            default:
                NotParsed = true;
                break;
        }
    }

    public string SurfaceType { get; set; }

    public string SurfaceCondition { get; set; }

    public string Lighting { get; set; }

    public string RoadElements { get; set; }

    public string Constructions { get; set; }

    public string Weather { get; set; }

    public List<string> TrafficTools { get; set; } = new();

    public bool? IsPlaceOfAccidentConcentration { get; set; }

    public bool NotParsed { get; set; }
}