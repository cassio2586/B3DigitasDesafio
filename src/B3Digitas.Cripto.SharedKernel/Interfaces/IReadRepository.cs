using Ardalis.Specification;

namespace B3Digitas.Cripto.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
