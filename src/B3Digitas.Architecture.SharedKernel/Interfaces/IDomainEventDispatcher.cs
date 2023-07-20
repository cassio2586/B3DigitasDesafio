
namespace B3Digitas.Architecture.SharedKernel.Interfaces;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents);
}
