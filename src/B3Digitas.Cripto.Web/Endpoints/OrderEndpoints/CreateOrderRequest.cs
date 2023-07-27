using System.ComponentModel.DataAnnotations;
using B3Digitas.Cripto.Core.OrderBookAggregate.Enums;

namespace B3Digitas.Cripto.Web.Endpoints.CashEndpoints;

public class CreateOrderRequest
{
    public const string Route = "/Order";
    public string? Symbol { get; set; }
    public string? Side{get;set;}
    public decimal Amount{get;set;}

}