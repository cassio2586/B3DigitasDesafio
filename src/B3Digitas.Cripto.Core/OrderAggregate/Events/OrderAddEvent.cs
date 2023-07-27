using B3Digitas.Cripto.SharedKernel;

namespace B3Digitas.Cripto.Core.OrderBookAggregate.Events;

public class OrderAddEvent : DomainEventBase
{   
    public Order Order { get; set; }
    public OrderAddEvent(Order order)
    {
        Order = order;
    }
}