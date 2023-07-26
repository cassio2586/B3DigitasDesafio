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

public class CreateOrderValuesService : ICreateOrderValuesService
{
  private readonly IRepository<Order> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateBookValuesService> _logger;
  private readonly IMemoryCache _cache;

  public CreateOrderValuesService(IRepository<Order> repository, IMemoryCache cache, IMediator mediator, ILogger<CreateBookValuesService> logger)
  {
    _repository = repository;
    _mediator = mediator;
    _logger = logger;
    _cache = cache;
  }
  public async Task<Result> Add(Order order)
  {
    if (order is null)
      throw new ArgumentNullException();


    var orderAggregate = order;
    var orderAdded = await _repository.AddAsync(orderAggregate);
    await _repository.SaveChangesAsync();
    var domainEvent = new OrderAddEvent(order);
    await _mediator.Publish(domainEvent);
    _logger.LogInformation("Transaction success saved.");

    return Result.Success();
  }
}
