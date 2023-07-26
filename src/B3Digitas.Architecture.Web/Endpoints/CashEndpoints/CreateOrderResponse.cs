using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateOrderResponse
{
    public string? Guid { get; set; }
    public string? Symbol { get; set; }
    public decimal BestPrice { get; set; }
    public decimal Amount { get; set; }
    public string? Side { get; set; }
    public List<BookLevel>? BookLevels { get; set; }
}