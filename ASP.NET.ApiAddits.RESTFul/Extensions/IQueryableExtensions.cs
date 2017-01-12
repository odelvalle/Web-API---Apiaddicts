using System.Linq;
using System.Linq.Dynamic;

namespace ASP.NET.ApiAddits.RESTFul.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            var lstSort = sort.Split(',');

            string completeSortExpression = "";
            foreach (var sortOption in lstSort)
            {
                if (sortOption.StartsWith("-"))
                {
                    completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";                                        
                }
                else
                {
                    completeSortExpression = completeSortExpression + sortOption + ",";               
                }

            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count()-1));
            }
         
            return source;
        }

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> source, string where)
        {
            if (where.IsNullOrTrimIsEmpty())
            {
                return source;
            }

            return source.Where(where);
        }

        public static IQueryable ApplySelect<T>(this IQueryable<T> source, string select)
        {
            if (select.IsNullOrTrimIsEmpty())
            {
                return source;
            }

            return source.Select($"new ({select})");
        }
    }
}