using Ardalis.Result;
using B3Digitas.Architecture.Core.CashAggregate.Enums;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface ICreateCashService
{
  public Task<Result> Add(string description, decimal amount, TransactionTypeEnum transactionType, DateTime dateTimeTransaction);
}
