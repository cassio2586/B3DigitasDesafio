using B3Digitas.Cripto.Core.OrderBookAggregate;

namespace B3Digitas.Cripto.Core.Interfaces;

public interface IGetOrderBookService
{
  IEnumerable<OrderBook> GetBySymbol(string symbol);
}
