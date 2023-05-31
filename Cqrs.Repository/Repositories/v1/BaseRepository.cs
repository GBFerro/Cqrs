using System.Linq.Expressions;
using Cqrs.Domain.Contracts.v1;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cqrs.Repository.Repositories.v1;

public abstract class BaseRepository<TEntity> where TEntity : IEntity
{
    protected BaseRepository(IMongoClient client, IOptions<MongoRepositorySettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    protected readonly IMongoCollection<TEntity> Collection;

    public async Task AddAsync(TEntity entity, CancellationToken cancellation)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellation);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        var filter = Builders<TEntity>.Filter.Where(expression);

        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<TEntity> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var filter = GetFilterById(id);

        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken)
    {
        var filter = GetFilterById(entity.Id);
        await Collection.ReplaceOneAsync(
            filter,
            entity,
            cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var filter = GetFilterById(id);

        await Collection.DeleteOneAsync(
            filter,
            cancellationToken);
    }

    protected FilterDefinition<TEntity> GetFilterById(Guid id) => Builders<TEntity>.Filter.Eq(entity => entity.Id, id);
}
