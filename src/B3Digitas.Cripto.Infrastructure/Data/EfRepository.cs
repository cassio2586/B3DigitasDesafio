using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using B3Digitas.Cripto.SharedKernel.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace B3Digitas.Cripto.Infrastructure.Data;

public class EfRepository<T> : EfRepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
