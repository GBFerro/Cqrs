using System.Linq.Expressions;
using Cqrs.Domain.Domain;

namespace Cqrs.Domain.Contracts;

public interface IPersonRepository
{
    Task InsertAsync(
        Person person,
        CancellationToken cancellationToken);

    Task<IEnumerable<Person>> GetAsync(
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken);

    Task<Person> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Person person,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);
}
