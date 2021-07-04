using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.Common.DataTables
{
    public static class DataTablesExtensions
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, DataTablesParameters parameters)
        {
            parameters.TotalCount = source.Count();
            return source.Skip(parameters.Start).Take(parameters.Length);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, DataTablesParameters parameters)
        {
            parameters.SetColumnName();
            var expression = source.Expression;
            var count = 0;
            foreach (var item in parameters.Order)
            {
                // x
                var parameter = Expression.Parameter(typeof(T), "x");
                // x.Name
                var selector = Expression.PropertyOrField(parameter, item.Name);
                
                var method = item.Dir.ToLower() == "desc" ?
                    (count == 0 ? nameof(Queryable.OrderByDescending) : nameof(Queryable.ThenByDescending)) :
                    (count == 0 ? nameof(Queryable.OrderBy) : nameof(Queryable.ThenBy));

                // OderBy(x => x.InstCode)
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }

}
