using B3Digitas.Architecture.Core.OrderBookAggregate.Events;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.OrderBookAggregate.Handlers;

public class OrderBookAddHandler : INotificationHandler<OrderBookAddEvent>
{
  private readonly ILogger<OrderBookAddHandler> _logger;
  private readonly IMemoryCache _cache;
  public OrderBookAddHandler(ILogger<OrderBookAddHandler> logger, IMemoryCache cache)
  {
    _logger = logger;
    _cache = cache;
  }

  public Task Handle(OrderBookAddEvent domainEvent, CancellationToken cancellationToken)
  {
    var histOrderBook = _cache.Get<List<OrderBook>>($"{domainEvent.OrderBook.Symbol}HistOrderBook");

    if (histOrderBook is null || histOrderBook.Count() == 0)
      histOrderBook = new List<OrderBook>();
    
    histOrderBook.Add(domainEvent.OrderBook);

    var avgHistOrderBook = new List<decimal>();

    var remove = histOrderBook.Where(a=>a.Timestamp < DateTime.Now.AddSeconds(-5));
    if (remove.Count() > 0)
    {
      foreach (var orderBookToRemove in remove)
      {
        histOrderBook.Remove(orderBookToRemove);
      }
    }
      
    
    foreach (var orderBook in histOrderBook)
    {
      avgHistOrderBook.Add((decimal)orderBook.BookLevels.Average(a=>a.Price));
    }
    if(avgHistOrderBook.Count > 0)
      domainEvent.OrderBook.FiveSecondAvgPrice = avgHistOrderBook.Average();
    
    //_cache.Set($"{domainEvent.OrderBook.Symbol}FiveSecondAvgPrice", avgHistOrderBook.Average());
    _cache.Set($"{domainEvent.OrderBook.Symbol}OrderBook", domainEvent.OrderBook);
    _cache.Set($"{domainEvent.OrderBook.Symbol}HistOrderBook", histOrderBook);
    return Task.CompletedTask;
  }
}
