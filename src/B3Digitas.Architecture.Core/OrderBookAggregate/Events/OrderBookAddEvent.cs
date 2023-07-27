using B3Digitas.Architecture.SharedKernel;

namespace B3Digitas.Architecture.Core.OrderBookAggregate.Events;

public class OrderBookAddEvent : DomainEventBase
{   
    public OrderBook OrderBook { get; set; }
    public OrderBookAddEvent(OrderBook orderBook)
    {
        OrderBook = orderBook;
    }
}