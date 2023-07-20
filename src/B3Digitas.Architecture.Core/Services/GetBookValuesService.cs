using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class GetBookValuesService : IGetBookValuesService
{
    private  IRepository<OrderBook> _repository;
    private  ILogger<GetBookValuesService> _logger;

    public GetBookValuesService(IRepository<OrderBook> repository,
        ILogger<GetBookValuesService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public BookValuesReport GetByInstrument( string instrument)
    {
        return new Result<BookValuesReport>(new BookValuesReport(instrument));
    }
    
/*
    public CashFlowDayReport Get(DateTime date)
    {
        var amount = (decimal)(_cache.Get(date.Date)??decimal.Zero);
        
        _logger.LogInformation($"Report generated to date {date.ToString(CultureInfo.InvariantCulture)}");
        
        return new Result<CashFlowDayReport>(new CashFlowDayReport(date, amount));
    }*/
}