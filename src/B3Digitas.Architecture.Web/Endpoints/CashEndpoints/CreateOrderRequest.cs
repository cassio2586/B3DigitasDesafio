using System.ComponentModel.DataAnnotations;
using B3Digitas.Architecture.Core.OrderBookAggregate.Enums;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateOrderRequest
{
    public const string Route = "/Order";
    public string? Symbol { get; set; }

    public string? Side{get;set;}
    
    public decimal? Qtd{get;set;}

}