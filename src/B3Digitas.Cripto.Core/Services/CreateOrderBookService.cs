using Ardalis.Result;
using B3Digitas.Cripto.Core.OrderBookAggregate.Enums;
using B3Digitas.Cripto.Core.Interfaces;
using B3Digitas.Cripto.Core.OrderBookAggregate;
using B3Digitas.Cripto.Core.OrderBookAggregate.Events;
using B3Digitas.Cripto.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Cripto.Core.Services;

public class CreateOrderBookService : ICreateOrderBookService
{
  private readonly IRepository<OrderBook> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateOrderBookService> _logger;
  private readonly IMemoryCache _cache;

  public CreateOrderBookService(IRepository<OrderBook> repository, IMemoryCache cache, IMediator mediator, ILogger<CreateOrderBookService> logger)
  {
    _repository = repository;
    _mediator = mediator;
    _logger = logger;
    _cache = cache;
  }
  public async Task<Result> Add(OrderBook orderBook)
  {
    if (orderBook is null)
      throw new ArgumentNullException();

    if(orderBook.BookLevels is null)
      throw new ArgumentNullException();
    
    var domainEvent = new OrderBookAddEvent(orderBook);
    await _mediator.Publish(domainEvent);
    _logger.LogInformation("Transaction success saved.");

    return Result.Success();
  }
}
