using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel;

namespace B3Digitas.Architecture.Core.OrderBookAggregate.Events;

public class OrderAddEvent : DomainEventBase
{   
    public Order Order { get; set; }
    public OrderAddEvent(Order order)
    {
        Order = order;
    }
}