using Ardalis.Result;
using B3Digitas.Cripto.Core.OrderBookAggregate;

namespace B3Digitas.Cripto.Core.Interfaces;

public interface ICreateOrderBookService
{
  public Task<Result> Add(OrderBook orderBook);
}
