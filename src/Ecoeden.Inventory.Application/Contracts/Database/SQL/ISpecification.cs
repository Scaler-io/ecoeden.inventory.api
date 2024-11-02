using Ecoeden.Inventory.Domain.Entities.SQL;
using System.Linq.Expressions;

namespace Ecoeden.Inventory.Application.Contracts.Database.SQL;
public interface ISpecification<TEntity> where TEntity : SQLBaseEntity
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<TEntity, object>> OrderBy { get; }
    Expression<Func<TEntity, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
