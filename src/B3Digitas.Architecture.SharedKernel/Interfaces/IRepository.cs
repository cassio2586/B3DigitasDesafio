using Ardalis.Specification;

namespace B3Digitas.Architecture.SharedKernel.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
