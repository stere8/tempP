namespace OnlineShop.Data
{
    public enum CartStatus
    {
        Active = 1,
        Deleted = 2,
        Paid = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4
    }

    public enum PaymentMethod
    {
        CreditCard = 1,
        DebitCard = 2,
        BankTransfer = 3,
        Cash = 4
    }
}
