using Ardalis.Result;
using B3Digitas.Architecture.Core.OrderBookAggregate;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface IGetBookValuesService
{
  IEnumerable<OrderBook> GetBySymbol(string symbol);
  IEnumerable<OrderBook> GetBySymbolLastFiveSeconds(string? symbol);
  decimal GetMaxPrice(string symbol);
  decimal GetMinPrice(string symbol);
  decimal GetAvgPriceLastFiveSeconds(string symbol);
  decimal GetAvgQtd(string symbol);
}
