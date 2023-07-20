using Ardalis.Result;
using B3Digitas.Architecture.Core.CashAggregate;
using B3Digitas.Architecture.Core.CashAggregate.Enums;
using B3Digitas.Architecture.Core.CashAggregate.Events;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class CreateCashService : ICreateCashService
{
  private readonly IRepository<Cash> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateCashService> _logger;

  public CreateCashService(IRepository<Cash> repository, IMediator mediator, ILogger<CreateCashService> logger)
  {
    _repository = repository;
    _mediator = mediator;
    _logger = logger;
  }
  public async Task<Result> Add(string description, decimal amount, TransactionTypeEnum transactionType, DateTime dateTimeTransaction)
  {
    if (description is null)
      throw new InvalidDataException();

    var cashAggregate = new Cash(description, amount, transactionType, dateTimeTransaction);
    var cashAdded = await _repository.AddAsync(cashAggregate);
    var domainEvent = new CashAddEvent(cashAdded.Id, cashAdded.Amount, transactionType, cashAdded.DateTimeTransaction);
    await _mediator.Publish(domainEvent);
    _logger.LogInformation("Transaction success saved.");
    return Result.Success();
  }
}
