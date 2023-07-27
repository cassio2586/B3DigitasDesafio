using System.ComponentModel.DataAnnotations.Schema;
using B3Digitas.Cripto.SharedKernel;
using B3Digitas.Cripto.SharedKernel.Interfaces;

namespace B3Digitas.Cripto.Core.OrderBookAggregate;

public class OrderBook : EntityBase, IAggregateRoot
{
    public string? Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    [NotMapped]
    public decimal FiveSecondAvgPrice { get; set; }
    public List<BookLevel> BookLevels { get; set; } = null!;
    public OrderBook()
    {
    }
    public decimal GetMaxPrice()
    {
        return (decimal)BookLevels.Where(x => x.BookType.Equals(1)).Max(x=>x.Price);
    }
    public decimal GetMinPrice()
    {
        return (decimal)BookLevels.Where(x => x.BookType.Equals(1)).Min(x=>x.Price);
    }
    public decimal GetAvgPrice()
    {
        return (decimal)BookLevels.Where(x => x.BookType.Equals(1)).Average(x=>x.Price);
    }
    public decimal GetAvgQtd()
    {
        return (decimal)BookLevels.Where(x => x.BookType.Equals(1)).Average(x=>x.Amount);
    }
}