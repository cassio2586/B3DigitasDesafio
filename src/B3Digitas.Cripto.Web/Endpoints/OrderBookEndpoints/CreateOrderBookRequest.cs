namespace B3Digitas.Cripto.Web.Endpoints.CashEndpoints;
public class CreateOrderBookRequest
{
    public const string Route = "/OrderBook";
    public string? Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    public List<BookLevelRequest>? BookLevels { get; set; }
}

public enum OrderBookSideRequest
{
    Undefined,
    Bid,
    Ask,
}

public class BookLevelRequest
{
    public OrderBookSideRequest SideRequest { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; }
    public long OrderId { get; set; }
    public int BookType { get; set; }
}

