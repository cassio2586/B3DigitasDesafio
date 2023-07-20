using Ardalis.Result;
using B3Digitas.Architecture.Core.CashAggregate;

namespace B3Digitas.Architecture.Core.Interfaces;

public interface IGetCashFlowDayReportService
{
  CashFlowDayReport Get(DateTime date);
}
