using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Domain.Entities.SQL;
using System.Linq.Expressions;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Specification;
public class BaseSpecification<TEntity> : ISpecification<TEntity>
    where TEntity : SQLBaseEntity
{
    public BaseSpecification()
    {

    }

    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }
    public Expression<Func<TEntity, bool>> Criteria { get; }

    public List<string> IncludeStrings { get; } = [];

    public Expression<Func<TEntity, object>> OrderBy { get; private set; }

    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    protected void AddIncludes(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Take = take;
        Skip = skip;
        IsPagingEnabled = true;
    }
}
