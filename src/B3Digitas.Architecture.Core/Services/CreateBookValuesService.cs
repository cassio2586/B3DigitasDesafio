using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;
using B3Digitas.Architecture.Core.OrderBookAggregate.Events;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class CreateBookValuesService : ICreateBookValuesService
{
  private readonly IRepository<OrderBook> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateBookValuesService> _logger;

  public CreateBookValuesService(IRepository<OrderBook> repository, IMediator mediator, ILogger<CreateBookValuesService> logger)
  {
    _repository = repository;
    _mediator = mediator;
    _logger = logger;
  }
  public async Task<Result> Add(OrderBook orderBook)
  {
    if (orderBook is null)
      throw new ArgumentNullException();
    
    if(orderBook.Asks is null)
      throw new ArgumentNullException();
    
    if(orderBook.Bids is null)
      throw new ArgumentNullException();

    var orderBookAggregate = orderBook;
    var orderBookAdded = await _repository.AddAsync(orderBookAggregate);
    var domainEvent = new OrderBookAddEvent(orderBookAdded);
    await _mediator.Publish(domainEvent);
    _logger.LogInformation("Transaction success saved.");
    return Result.Success();
  }
}
