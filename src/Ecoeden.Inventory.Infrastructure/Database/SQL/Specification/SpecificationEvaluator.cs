using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Specification;
public class SpecificationEvaluator<TEntity> where TEntity : SQLBaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;
        if (spec.Criteria is not null) query = query.Where(spec.Criteria);
        if (spec.OrderBy is not null) query = query.OrderBy(spec.OrderBy);
        if (spec.OrderByDescending is not null) query = query.OrderByDescending(spec.OrderByDescending);
        if (spec.IsPagingEnabled) query = query.Skip(spec.Skip).Take(spec.Take);
        if (spec.IncludeStrings is not null && spec.IncludeStrings.Count > 0) query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
