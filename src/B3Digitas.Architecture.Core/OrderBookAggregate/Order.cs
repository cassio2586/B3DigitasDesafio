using System.ComponentModel.DataAnnotations.Schema;
using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class Order : EntityBase, IAggregateRoot
{
    public string? Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    
    public string? Side{get;set;}
    
    public decimal? Amount{get;set;}

    public decimal? Qtd{get;set;}

    public string? HashId{get;set;}


    public Order()
    {
        
    }
}