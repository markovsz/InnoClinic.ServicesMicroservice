using Domain.Entities;
using static Dapper.SqlMapper;

namespace Infrastructure;

public class EntityBuilder<TEntity> : IEntityBuilder<TEntity>, IDisposable where TEntity : BaseEntity 
{
    private readonly GridReader _gridReader;
    private IEnumerable<TEntity> _entities;

    public EntityBuilder(GridReader gridReader)
    {
        _gridReader = gridReader;
    }

    public async Task RetrieveBaseEntityAsync()
    {
        _entities = await _gridReader.ReadAsync<TEntity>();
    }

    public async Task JoinRelatedEntityAsync<TRelatedEntity>(Func<TEntity, Guid> keySelector, Func<TEntity, TRelatedEntity, TEntity> joinCondition) where TRelatedEntity : BaseEntity
    {
        var relatedEntities = await _gridReader.ReadAsync<TRelatedEntity>();
        _entities = _entities.Join(relatedEntities, keySelector, re => re.Id, joinCondition);
    }

    public IEnumerable<TEntity> GetEntities()
    {
        return _entities;
    }

    public void Dispose()
    {
        _gridReader.Dispose();
    }
}
