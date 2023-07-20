using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class OrderBook : EntityBase, IAggregateRoot
{
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    public List<BookLevel> Bids { get; set; } = null!;
    public List<BookLevel> Asks { get; set; } = null!;
    
    public OrderBook()
    {
        
    }

    public OrderBook(DateTime timestamp, DateTime microtimestamp, List<BookLevel> bids, List<BookLevel> asks)
    {
        Timestamp = timestamp;
        Microtimestamp = microtimestamp;
        Bids = bids;
        Asks = asks;
    }
}