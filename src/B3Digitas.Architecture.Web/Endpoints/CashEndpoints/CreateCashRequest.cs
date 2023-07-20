using System.ComponentModel.DataAnnotations;
using B3Digitas.Architecture.Core.CashAggregate.Enums;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateCashRequest
{
  public const string Route = "/Cash";

  [Required]
  public string? Description { get; set; }
  
  [Required]
  public decimal Amount { get; set; }
  
  [Required]
  public TransactionTypeEnum TransactionType { get; set; }
  
  [Required]
  public DateTime DateTimeTransaction { get; set; }
}
