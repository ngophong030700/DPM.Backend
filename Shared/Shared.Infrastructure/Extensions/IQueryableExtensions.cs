using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(param, propertyName);
            var lambda = Expression.Lambda(property, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == methodName
                         && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { query, lambda });

            return (IQueryable<T>)result!;
        }
    }
}
