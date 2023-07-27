using B3Digitas.Architecture.Core.OrderBookAggregate;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface IGetBookValuesService
{
  IEnumerable<OrderBook> GetBySymbol(string symbol);
}
