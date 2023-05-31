using System.Linq.Expressions;

namespace Cqrs.Domain.Contracts.v1;

public interface IBaseRepository<TEntity> where TEntity : IEntity
{
    Task AddAsync(
    TEntity entity,
    CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>?> FindAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken);

    Task<TEntity?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken);

    Task RemoveAsync(
        Guid id,
        CancellationToken cancellationToken);
}
