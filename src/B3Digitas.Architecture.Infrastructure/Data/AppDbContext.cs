using System.Reflection;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace B3Digitas.Architecture.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }
  
  public DbSet<OrderBook> OrderBook => Set<OrderBook>();
  public DbSet<BookLevel> BookLevel => Set<BookLevel>();
  public DbSet<Order> Order => Set<Order>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    
    modelBuilder.Entity<BookLevel>()
      .HasOne<OrderBook>()
      .WithMany(s=>s.BookLevels)
      .HasForeignKey(e=>e.OrderBookId);

  }
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    
    if (_dispatcher == null) return result;
    
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }
  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
