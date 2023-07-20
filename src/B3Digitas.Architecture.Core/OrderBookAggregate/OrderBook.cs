using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class OrderBook : EntityBase, IAggregateRoot
{
    public string? Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    public List<BookLevel> Details { get; set; } = null!;
    
    public OrderBook()
    {
        
    }

    public OrderBook(string symbol, DateTime timestamp, DateTime microtimestamp, List<BookLevel> details, List<BookLevel> asks)
    {
        Symbol = symbol;
        Timestamp = timestamp;
        Microtimestamp = microtimestamp;
        Details = details;
    }
}