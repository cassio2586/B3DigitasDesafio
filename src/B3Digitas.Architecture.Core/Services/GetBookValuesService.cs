using System.Collections;
using System.Globalization;
using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Core.Services;

public class GetBookValuesService : IGetBookValuesService
{
    private IRepository<BookLevel> _repositoryBookLevel;
    private IRepository<OrderBook> _repositoryOrderBook;
    private ILogger<GetBookValuesService> _logger;
    private readonly IMemoryCache _cache;

    public GetBookValuesService(IRepository<BookLevel> repositoryBookLevel,
        IRepository<OrderBook> repositoryOrderBook,
        ILogger<GetBookValuesService> logger, IMemoryCache cache)
    {
        _repositoryOrderBook = repositoryOrderBook;
        _repositoryBookLevel = repositoryBookLevel;
        _cache = cache;
        _logger = logger;
    }


    public IEnumerable<OrderBook> GetBySymbolLastFiveSeconds(string? symbol)
    {
        throw new NotImplementedException();
    }

    public decimal GetMaxPrice(string symbol)
    {
        throw new NotImplementedException();
    }

    public decimal GetMinPrice(string symbol)
    {
        throw new NotImplementedException();
    }

    public decimal GetAvgPriceLastFiveSeconds(string symbol)
    {
        return 0;
        /*var lastFiveSecondsOrderBooks = _repositoryOrderBook.ListAsync().Result.Where(a =>
            a.Symbol != null && a.Symbol.Equals(symbol) && a.Timestamp >= DateTime.Now.AddSeconds(-5)).ToList();

        List<decimal> avgList = new List<decimal>();
        foreach (var orderBook in lastFiveSecondsOrderBooks)
        {
            
            if (orderBook != null)
            {
                var avg  = (decimal)_repositoryBookLevel.ListAsync().Result.Where(x => x.OrderBookId == orderBook.Id)
                    .ToList().Average(a => a.Price);
                avgList.Add((avg));

            }
            
        }

        if (avgList.Count == 0)
            return 0;
        
        return avgList.Average();*/
    }

    public decimal GetAvgQtd(string symbol)
    {
        throw new NotImplementedException();
    }


    IEnumerable<OrderBook> IGetBookValuesService.GetBySymbol(string? symbol)
    {
        var orderBookList = new List<OrderBook>();
        OrderBook? orderBook = _cache.Get<OrderBook>($"{symbol}OrderBook");
        if (orderBook != null) orderBookList.Add(orderBook);

        return orderBookList;
    }
}