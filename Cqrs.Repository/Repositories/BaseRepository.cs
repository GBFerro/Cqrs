using Cqrs.Domain.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cqrs.Repository.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected BaseRepository(IMongoClient client, IOptions<MongoRepositorySettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    protected readonly IMongoCollection<TEntity> Collection;
}
