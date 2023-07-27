using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;
using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class BookLevel : EntityBase, IAggregateRoot
{
    public OrderBookSide Side { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; }
    public long OrderId { get; set; }
    public int BookType { get; set; }
    
    public int OrderBookId { get; set; }
    
    public OrderBook? OrderBook { get; set; }
}
