namespace Northwind.Orders.WebApi.Models;

public class BriefOrder
{
    public long Id { get; set; }

    public string CustomerId { get; init; } = default!;

    public long EmployeeId { get; init; }

    public DateTime OrderDate { get; init; }

    public DateTime RequiredDate { get; init; }

    public DateTime? ShippedDate { get; init; }

    public long ShipperId { get; init; }

    public double Freight { get; init; }

    public string ShipName { get; init; } = default!;

    public string ShipAddress { get; init; } = default!;

    public string ShipCity { get; init; } = default!;

    public string? ShipRegion { get; init; }

    public string ShipPostalCode { get; init; } = default!;

    public string ShipCountry { get; init; } = default!;

    public IList<BriefOrderDetail> OrderDetails { get; init; } = default!;
}
