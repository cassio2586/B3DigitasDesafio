using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class BookValuesReport : ValueObject, IAggregateRoot
{
  public string Instrument { get; private set; }
  
  public BookValuesReport(string instrument)
  {
    Instrument = instrument;
  }
  protected override IEnumerable<object> GetEqualityComponents()
  {
    throw new NotImplementedException();
  }
}
