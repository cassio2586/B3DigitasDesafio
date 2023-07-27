using B3Digitas.Cripto.Core.OrderBookAggregate.Enums;
using B3Digitas.Cripto.Core.OrderBookAggregate;

namespace B3Digitas.Cripto.Web.Endpoints.CashEndpoints;

public class CreateOrderResponse
{
    public string? Guid { get; set; }
    public string? Symbol { get; set; }
    public decimal BestPrice { get; set; }
    public decimal Amount { get; set; }
    public string? Side { get; set; }
    public List<BookLevel>? BookLevels { get; set; }
}