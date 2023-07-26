using System.ComponentModel.DataAnnotations.Schema;
using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Core.OrderBookAggregate;

public class OrderBook : EntityBase, IAggregateRoot
{
    public string? Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Microtimestamp { get; set; }
    
   
    public List<BookLevel> BookLevels { get; set; } = null!;
    
    [NotMapped]
    public decimal FiveSecondAvgPrice { get; set; }

    public OrderBook()
    {
        
    }
    public OrderBook(string symbol, DateTime timestamp, DateTime microtimestamp, List<BookLevel> bookLevels, List<BookLevel> asks)
    {
        Symbol = symbol;
        Timestamp = timestamp;
        Microtimestamp = microtimestamp;
        BookLevels = bookLevels;
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