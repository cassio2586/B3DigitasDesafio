using System.ComponentModel.DataAnnotations;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class GetAllOrderBookBySymbolRequest
{
  public const string Route = "/GetAllOrderBookBySymbol";

  [Required]
  public string? Symbol { get; set; }
  
}
