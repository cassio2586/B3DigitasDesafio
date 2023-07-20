using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.CashAggregate;

public class CashFlowDayReport : ValueObject, IAggregateRoot
{
  public DateTime Day { get; private set; }
  public decimal Amount { get; private set; }

  public CashFlowDayReport(DateTime day, decimal amount)
  {
    Day = day;
    Amount = amount;
  }
  protected override IEnumerable<object> GetEqualityComponents()
  {
    throw new NotImplementedException();
  }
}
