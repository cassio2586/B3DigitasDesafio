using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;
using B3Digitas.Architecture.Core.OrderBookAggregate.Events;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class CreateBookValuesService : ICreateBookValuesService
{
  private readonly IRepository<OrderBook> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateBookValuesService> _logger;
  private readonly IMemoryCache _cache;

  public CreateBookValuesService(IRepository<OrderBook> repository, IMemoryCache cache, IMediator mediator, ILogger<CreateBookValuesService> logger)
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

    //var orderBookAggregate = orderBook;
    //var orderBookAdded = await _repository.AddAsync(orderBookAggregate);
    //await _repository.SaveChangesAsync();
    var domainEvent = new OrderBookAddEvent(orderBook);
    await _mediator.Publish(domainEvent);
    _logger.LogInformation("Transaction success saved.");

    return Result.Success();
  }

  public void Purge(string? symbol)
  {
    throw new NotImplementedException();
  }

  public void Purge()
  {
    
    var bookOrders = _repository.ListAsync().Result.Where(a => a.Timestamp < DateTime.Now.AddSeconds(10));
    _repository.DeleteRangeAsync(bookOrders);
    _repository.SaveChangesAsync();
  }
}
