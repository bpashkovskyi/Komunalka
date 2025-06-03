namespace Advertising.Model;

public class Board
{
    public int Id { get; set; }

    public string Identifier { get; set; }
    public DateTime? Issued { get; set; }
    public string AuthorityName { get; set; }
    public string AuthorityIdentifier { get; set; }
    public string DistributorName { get; set; }
    public string NormalizedDistributorName { get; set; }
    public string DistributorIdentifier { get; set; }
    public string Status { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidThrough { get; set; }
    public string ContractNumber { get; set; }
    public DateTime? ContractDateSigned { get; set; }
    public string Type { get; set; }
    public string NormalizedType { get; set; }
    public double? PlanesValue { get; set; }
    public double? HorizontalSizeValue { get; set; }
    public double? VerticalSizeValue { get; set; }
    public double? SquareValue { get; set; }

    public Address Address { get; set; }
    public Coordinates Coordinates { get; set; }
    public string ImageURL { get; set; }
}