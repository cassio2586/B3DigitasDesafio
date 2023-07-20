using Ardalis.Specification.EntityFrameworkCore;
using B3Digitas.Architecture.SharedKernel.Interfaces;

namespace B3Digitas.Architecture.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
