using System.Globalization;
using Ardalis.Result;
using B3Digitas.Architecture.Core.CashAggregate;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.CashAggregate.Enums;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class GetCashFlowDayReportService : IGetCashFlowDayReportService
{
    private readonly IRepository<Cash> _repository;
    private readonly ILogger<GetCashFlowDayReportService> _logger;
    private readonly IMemoryCache _cache;

    public GetCashFlowDayReportService(IRepository<Cash> repository,
        ILogger<GetCashFlowDayReportService> logger,
        IMemoryCache cache)
    {
        _repository = repository;
        _logger = logger;
        _cache = cache;
    }

    public CashFlowDayReport Get(DateTime date)
    {
        var amount = (decimal)(_cache.Get(date.Date)??decimal.Zero);
        
        _logger.LogInformation($"Report generated to date {date.ToString(CultureInfo.InvariantCulture)}");
        
        return new Result<CashFlowDayReport>(new CashFlowDayReport(date, amount));
    }
}