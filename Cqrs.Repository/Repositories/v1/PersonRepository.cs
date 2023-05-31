using System.Linq.Expressions;
using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Entities.v1;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cqrs.Repository.Repositories.v1;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(
        IMongoClient client,
        IOptions<MongoRepositorySettings> settings)
        : base(client, settings)
    {
    }
}
