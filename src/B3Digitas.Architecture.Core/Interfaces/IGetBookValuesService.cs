using Ardalis.Result;
using B3Digitas.Architecture.Core.OrderBookAggregate;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface IGetBookValuesService
{
  BookValuesReport GetByInstrument(string instrument);
}
