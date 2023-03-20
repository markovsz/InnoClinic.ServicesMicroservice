using Domain.Entities;

namespace Infrastructure;

public interface IEntityBuilder<TEntity> : IDisposable where TEntity : BaseEntity
{
    Task RetrieveBaseEntityAsync();
    Task JoinRelatedEntityAsync<TRelatedEntity>(Func<TEntity, Guid> keySelector, Func<TEntity, TRelatedEntity, TEntity> joinCondition) where TRelatedEntity : BaseEntity;
    IEnumerable<TEntity> GetEntities();
}
