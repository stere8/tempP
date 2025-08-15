using System.Diagnostics;

namespace Northwind.Services.Repositories;

/// <summary>
/// Represents an order.
/// </summary>
[DebuggerDisplay("Order #{Id}")]
public class Order
{
    public Order(long id)
    {
        this.Id = id;
    }

    public long Id { get; private set; }

    public Customer Customer { get; init; } = default!;

    public Employee Employee { get; init; } = default!;

    public DateTime OrderDate { get; init; }

    public DateTime RequiredDate { get; init; }

    public DateTime? ShippedDate { get; init; }

    public Shipper Shipper { get; init; } = default!;

    public double Freight { get; init; }

    public string ShipName { get; init; } = default!;

    public ShippingAddress ShippingAddress { get; init; } = default!;

    public IList<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
