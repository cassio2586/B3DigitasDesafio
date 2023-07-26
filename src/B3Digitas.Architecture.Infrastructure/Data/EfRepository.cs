using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using B3Digitas.Architecture.SharedKernel.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace B3Digitas.Architecture.Infrastructure.Data;

public class EfRepository<T> : EfRepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
