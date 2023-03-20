using Dapper;
using Domain.Entities;

namespace Infrastructure;

public interface ISqlDataAccess
{
    Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string storedProcedure, DynamicParameters parameters);
    Task<IEntityBuilder<TEntity>> QueryComplexAsync<TEntity>(string storedProcedure, DynamicParameters parameters) where TEntity : BaseEntity;
    Task ExecuteAsync(string storedProcedure, DynamicParameters parameters);
}
