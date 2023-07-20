using Ardalis.Result;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;
using B3Digitas.Architecture.Core.OrderBookAggregate;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface ICreateBookValuesService
{
  public Task<Result> Add(OrderBook orderBook);
}
