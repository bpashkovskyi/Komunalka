namespace Accidents.Model;

public class Accident
{
    public Guid Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Type { get; set; }

    public List<string> Reasons { get; set; } = new();

    public Address Address { get; set; }

    public Environment Environment { get; set; }

    public List<string> Casualties { get; set; } = new();

    public Coordinates Coordinates { get; set; }

    public void AddReason(string reasonText)
    {
        if (reasonText == null)
        {
            return;
        };

        if (!reasonText.Contains(','))
        {
            Reasons.Add(reasonText);
        }

        var reasons = reasonText.Split(',');

        foreach (var reason in reasons)
        {
            switch (reason)
            {
                case "01":
                    Reasons.Add("Керування транспортним засобом у нетверезому стані");
                    break;
                case "02":
                    Reasons.Add("Перевищення встановленої швидкості");
                    break;
                case "03":
                    Reasons.Add("Перевищення безпечної швидкості");
                    break;
                case "04":
                    Reasons.Add("Невиконання вимог сигналів регулювання");
                    break;
                case "05":
                    Reasons.Add("Порушення правил перевезення пасажирів");
                    break;
                case "06":
                    Reasons.Add("Порушення правил маневрування");
                    break;
                case "07":
                    Reasons.Add("Порушення правил проїзду пішохідних переходів");
                    break;
                case "08":
                    Reasons.Add("Порушення правил проїзду зупинок громадського транспорту");
                    break;
                case "09":
                    Reasons.Add("Порушення правил користування освітлювальними приладами");
                    break;
                case "10":
                    Reasons.Add("Порушення правил надання безперешкодного проїзду");
                    break;
                case "11":
                    Reasons.Add("Порушення правил зупинки і стоянки транспортного засобу");
                    break;
                case "12":
                    Reasons.Add("Порушення правил проїзду залізничних переїздів");
                    break;
                case "13":
                    Reasons.Add("Порушення правил перевезення вантажів");
                    break;
                case "14":
                    Reasons.Add("Порушення правил буксирування");
                    break;
                case "15":
                    Reasons.Add("Порушення правил обгону");
                    break;
                case "16":
                    Reasons.Add("Виїзд на смугу зустрічного руху");
                    break;
                case "17":
                    Reasons.Add("Порушення правил проїзду перехресть");
                    break;
                case "18":
                    Reasons.Add("Управління несправним транспортним засобом");
                    break;
                case "19":
                    Reasons.Add("Недодержання дистанції");
                    break;
                case "20":
                    Reasons.Add("Перевтома, сон за кермом");
                    break;
                case "21":
                    Reasons.Add("Порушення правил проїзду великогабаритних та великовагових транспортних засобів");
                    break;
                case "22":
                    Reasons.Add("Перехід у невстановленому місці");
                    break;
                case "23":
                    Reasons.Add("Невиконання вимог сигналів регулювання");
                    break;
                case "24":
                    Reasons.Add("Неочікуваний вихід на проїзну частину");
                    break;
                case "25":
                    Reasons.Add("Пішохід у нетверезому стані");
                    break;
            }
        }
    }

    public bool PedestrianInvolved => Type.ToLower().Contains("пішох");

    public bool CyclistInvolved => Type.ToLower().Contains("велос");

    public int NotInjured => Casualties.Count(casualty => casualty == "Без ушкоджень");

    public int MinorInjuries => Casualties.Count(casualty => casualty == "Легко травмований");

    public int MajorInjuries => Casualties.Count(casualty => casualty == "Тяжко травмований");

    public int Dead => Casualties.Count(casualty => casualty.Contains("Помер"));
}