using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Project.Service.Data.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(
            this IQueryable<T> query, 
            string propertyName,
            string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) 
                ? "OrderByDescending" 
                : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}