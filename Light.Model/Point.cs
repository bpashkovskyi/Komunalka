namespace Light.Model;

public class Point
{
    public Guid Id { get; set; }

    public Address Address { get; set; }

    public Coordinates Coordinates { get; set; }
    public string Queue { get; set; }
}