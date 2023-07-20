using B3Digitas.Architecture.Core.CashAggregate.Enums;
using B3Digitas.Architecture.Core.CashAggregate.Events;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.CashAggregate.Handlers;

public class CashAddHandler : INotificationHandler<CashAddEvent>
{
  private readonly ILogger<CashAddHandler> _logger;
  private readonly IMemoryCache _cache;
  public CashAddHandler(ILogger<CashAddHandler> logger, IMemoryCache cache)
  {
    _logger = logger;
    _cache = cache;
  }

  public Task Handle(CashAddEvent domainEvent, CancellationToken cancellationToken)
  {
    var amount = domainEvent.TransactionType.Equals(TransactionTypeEnum.Debit)
      ? domainEvent.Amount * -1
      : domainEvent.Amount;

    var amountSum = (decimal)(_cache.Get(domainEvent.DateTimeTransaction.Date) ?? decimal.Zero);
    _cache.Set(domainEvent.DateTimeTransaction.Date, amountSum + amount);

    return Task.CompletedTask;
  }
}
