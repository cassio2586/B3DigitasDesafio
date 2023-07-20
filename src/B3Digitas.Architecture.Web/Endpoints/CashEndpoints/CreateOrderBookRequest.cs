using System.ComponentModel.DataAnnotations;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateOrderBookRequest
{
  public const string Route = "/OrderBook";
  
  public DateTime Timestamp { get; set; }
  public DateTime Microtimestamp { get; set; }
  public List<BookLevelRequest>? Bids { get; set; }
  public List<BookLevelRequest>? Asks { get; set; }
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
}
