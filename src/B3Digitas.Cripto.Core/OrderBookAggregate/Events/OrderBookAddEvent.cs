using B3Digitas.Cripto.SharedKernel;

namespace B3Digitas.Cripto.Core.OrderBookAggregate.Events;

public class OrderBookAddEvent : DomainEventBase
{   
    public OrderBook OrderBook { get; set; }
    public OrderBookAddEvent(OrderBook orderBook)
    {
        OrderBook = orderBook;
    }
}