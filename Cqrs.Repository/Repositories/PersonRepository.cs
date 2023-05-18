using System.Linq.Expressions;
using Cqrs.Domain.Contracts;
using Cqrs.Domain.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cqrs.Repository.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(
        IMongoClient client, 
        IOptions<MongoRepositorySettings> settings) 
        : base(client, settings)
    {
    }

    public async Task InsertAsync(Person person, CancellationToken cancellation)
    {
        await Collection.InsertOneAsync(person, cancellationToken: cancellation);
    }

    public async Task<IEnumerable<Person>> GetAsync(
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Person>.Filter.Where(expression);

        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Person> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Person>.Filter.Eq(person => person.Id, id);

        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Person person,
        CancellationToken cancellationToken)
    {
        await Collection.ReplaceOneAsync(
            entity => entity.Id!.Equals(person.Id), 
            person, 
            new ReplaceOptions { IsUpsert = true}, 
            cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Person>.Filter.Eq(
            entity => entity.Id, id);

        await Collection.DeleteOneAsync(
            filter, 
            cancellationToken);
    }
}
