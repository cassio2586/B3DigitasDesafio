using B3Digitas.Architecture.Core.OrderBookAggregate;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class GetAllOrderBookBySymbolResponse
{
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal AvgPrice { get; set; }
    public decimal AvgQtd { get; set; }
    public decimal AvgPriceLastFiveSeconds { get; set; }
    public IEnumerable<OrderBook> OrderBooks { get; set; }

    public GetAllOrderBookBySymbolResponse(IEnumerable<OrderBook> orderBooks)
    {
        OrderBooks = orderBooks;
        MaxPrice = orderBooks.FirstOrDefault()!.GetMaxPrice();
        MinPrice = orderBooks.FirstOrDefault()!.GetMinPrice();
        AvgPrice = orderBooks.FirstOrDefault()!.GetAvgPrice();
        AvgPriceLastFiveSeconds = 0;
        AvgQtd = orderBooks.FirstOrDefault()!.GetAvgQtd();
    }
}