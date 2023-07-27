using Ardalis.Result;
using B3Digitas.Cripto.Core.OrderBookAggregate;

namespace B3Digitas.Cripto.Core.Interfaces;

public interface ICreateOrderService
{
  public Task<Result> Add(Order order);
}
