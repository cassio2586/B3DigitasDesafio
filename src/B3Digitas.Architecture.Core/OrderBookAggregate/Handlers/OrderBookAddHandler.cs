using B3Digitas.Architecture.Core.OrderBookAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.OrderBookAggregate.Handlers;

public class OrderBookAddHandler : INotificationHandler<OrderBookAddEvent>
{
  private readonly ILogger<OrderBookAddHandler> _logger;
  public OrderBookAddHandler(ILogger<OrderBookAddHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(OrderBookAddEvent domainEvent, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
