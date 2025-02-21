﻿using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Common.Helper
{
    public enum Comparison
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        In,
        Contains
    }

    public static class ExpressionLinq
    {
        private static readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(
                BindingFlags.Static | BindingFlags.Public)
                .Single(m => m.Name == nameof(Enumerable.Contains)
                    && m.GetParameters().Length == 2);
        //TO DO In future condition can be or too and dynamic.
        private delegate Expression Binder(Expression left, Expression right);
        private static readonly string Number = nameof(Number).ToLower();
        private static readonly string String = nameof(String).ToLower();
        public static JsonElement RealObject { get; set; }
        /// <summary>
        /// This method is used for dynamic linq filter 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">@this source of linq</param>
        /// <param name="search">list search key,value and condition </param>
        /// <returns></returns>
        public static IQueryable<T> filter<T>(this IQueryable<T> source, List<SearchParameter> search)
        {
            var stringProperties = typeof(T).GetProperties();
            var itemExpression = Expression.Parameter(typeof(T));
            Expression expression = null;
            foreach (var item in search)
            {
                //var d = JsonDocument.Parse(item.Value);
                if (stringProperties.Any(x => string.Equals(x.Name, item.ColumnSearchName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    Type propertyType = stringProperties.Where(x => string.Equals(x.Name, item.ColumnSearchName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().PropertyType;

                    MemberExpression property = Expression.Property(itemExpression, item.ColumnSearchName);

                    object val = GetValue(propertyType, item);
                    ConstantExpression toCompare = Expression.Constant(val);

                    if (propertyType != typeof(DateTime))
                    {
                        expression = NormalExpression(propertyType, property, item, toCompare);
                    }
                    else
                    {
                        expression = ExpressionForDateTime(propertyType, property, item, toCompare);
                    }

                    var lambda = Expression.Lambda<Func<T, bool>>(expression, itemExpression);
                    source = source.Where(lambda);
                }
            }
            return source;
        }


        /// <summary>
        /// This method create the Expression for DateType: string,Number,Decimal
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="property"></param>
        /// <param name="item"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static Expression NormalExpression(Type propertyType, MemberExpression property, SearchParameter item, ConstantExpression toCompare)
        {
            Expression expression = null;
            switch (true)
            {
                case bool when string.Equals(@item.Operator, Comparison.Equal.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.Equal(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.NotEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.NotEqual(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.GreaterThan.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.GreaterThan(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.GreaterThanOrEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.GreaterThanOrEqual(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.LessThan.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.LessThan(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.LessThanOrEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    expression = Expression.LessThanOrEqual(property, toCompare);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.In.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    ExpressionValue expressionValueIn = GetExpressionMethodInfoArray(propertyType);
                    expression = Expression.Call(expressionValueIn.Method, toCompare, property);
                    break;
                case bool when string.Equals(@item.Operator, Comparison.Contains.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    expression = Expression.Call(property, method, toCompare);
                    break;
                default:

                    break;
            }
            return expression;
        }

        /// <summary>
        /// This method create the Expressin for DataType: Datetime 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="property"></param>
        /// <param name="searchParameter"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static Expression ExpressionForDateTime(Type propertyType, MemberExpression property, SearchParameter searchParameter, ConstantExpression toCompare)
        {
            Expression expression = null;
            //TO DO In future condition can be or too and dynamic.
            //string gate = "And";
            //Binder binder = gate == "And" ? (Binder)Expression.And : Expression.Or;
            Binder binder = (Binder)Expression.Or;
            Expression bind(Expression left, Expression right) => left == null ? right : binder(left, right);

            JsonElement jsonElement = ((JsonElement)@searchParameter.Value);
            JsonValueKind type = jsonElement.ValueKind;

            DateTime dateTime;
            if (propertyType == typeof(DateTime) && type == JsonValueKind.String)
            {
                DateTime.TryParseExact(jsonElement.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime);
            }
            else
            {
                dateTime = DateTime.Now;
            }
            DateTime fromDate;
            DateTime toDate;
            Expression left = null;
            Expression right = null;
            switch (true)
            {
                case bool when string.Equals(@searchParameter.Operator, Comparison.Equal.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    fromDate = dateTime.StartOfDay();
                    toDate = dateTime.EndOfDay();
                    left = Expression.GreaterThan(property, Expression.Constant(fromDate));
                    right = Expression.LessThan(property, Expression.Constant(toDate));
                    expression = Expression.And(left, right);
                    break;
                //case bool when string.Equals(@item.Operator, Comparison.NotEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                //    expression = Expression.NotEqual(property, toCompare);
                //    break;
                case bool when string.Equals(@searchParameter.Operator, Comparison.GreaterThan.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    fromDate = dateTime.AddDays(1).StartOfDay();
                    expression = Expression.GreaterThan(property, Expression.Constant(fromDate));
                    break;
                case bool when string.Equals(@searchParameter.Operator, Comparison.GreaterThanOrEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    fromDate = dateTime.StartOfDay();
                    expression = Expression.GreaterThanOrEqual(property, Expression.Constant(fromDate));
                    break;
                case bool when string.Equals(@searchParameter.Operator, Comparison.LessThan.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    fromDate = dateTime.AddDays(-1).StartOfDay();
                    expression = Expression.LessThan(property, Expression.Constant(fromDate));
                    break;
                case bool when string.Equals(@searchParameter.Operator, Comparison.LessThanOrEqual.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    fromDate = dateTime.EndOfDay();
                    expression = Expression.LessThanOrEqual(property, Expression.Constant(fromDate));
                    break;
                case bool when string.Equals(@searchParameter.Operator, Comparison.In.ToString(), StringComparison.InvariantCultureIgnoreCase):
                    if (propertyType == typeof(DateTime) && type == JsonValueKind.Array)
                    {
                        string[] datetimeString = jsonElement.EnumerateArray().Select(o => o.GetString()).ToArray();
                        Expression rightExpression = null;
                        foreach (var item in datetimeString)
                        {
                            if (ValidationHelpers.IsDateValid(item))
                            {
                                DateTime.TryParseExact(item, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime searchDateTime);
                                fromDate = searchDateTime.StartOfDay();
                                toDate = searchDateTime.EndOfDay();
                                left = Expression.GreaterThan(property, Expression.Constant(fromDate));
                                right = Expression.LessThan(property, Expression.Constant(toDate));
                                rightExpression = Expression.And(left, right);
                                expression = bind(expression, rightExpression);
                            }
                        }
                    }
                    break;
                default:

                    break;
            }
            return expression;
        }

        /// <summary>
        /// This method get the value and cast according to datatype
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="searchParameter"></param>
        /// <returns></returns>
        public static object GetValue(Type propertyType, SearchParameter searchParameter)
        {
            object val = null;
            JsonElement jsonElement = ((JsonElement)searchParameter.Value);
            JsonValueKind type = jsonElement.ValueKind;

            if (propertyType == typeof(int) && type == JsonValueKind.Number)
            {
                val = jsonElement.GetInt32();
            }
            else if (propertyType == typeof(string) && type == JsonValueKind.String)
            {
                val = jsonElement.GetString();
            }
            else if (propertyType == typeof(int) && type == JsonValueKind.Array)
            {
                val = jsonElement.EnumerateArray().Select(o => o.GetInt32()).ToArray();
            }
            else if (propertyType == typeof(string) && type == JsonValueKind.Array)
            {
                val = jsonElement.EnumerateArray().Select(o => o.GetString()).ToArray();
            }
            else if (propertyType == typeof(DateTime) && type == JsonValueKind.String)
            {
                if (ValidationHelpers.IsDateValid(jsonElement.GetString()))
                {
                    DateTime.TryParseExact(jsonElement.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime);
                    val = dateTime;
                }
            }
            else if (propertyType == typeof(DateTime) && type == JsonValueKind.Array)
            {
                string[] datetimeString = jsonElement.EnumerateArray().Select(o => o.GetString()).ToArray();
                List<DateTime> dateTimeArray = new List<DateTime>();
                foreach (var item in datetimeString)
                {
                    if (ValidationHelpers.IsDateValid(item))
                    {
                        DateTime.TryParseExact(item, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime);
                        dateTimeArray.Add(dateTime);
                    }
                }
                val = dateTimeArray;
            }
            return val;
        }

        /// <summary>
        /// This method Create the Expression Method of Array for DateType Int and String
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static ExpressionValue GetExpressionMethodInfoArray(Type propertyType)
        {
            ExpressionValue expressionValue = new ExpressionValue();
            switch (true)
            {
                case bool when propertyType == typeof(int):
                    expressionValue.Method = MethodContains.MakeGenericMethod(typeof(int));
                    //int x = 0;
                    //expressionValue.Value = searchParameter.ColumnSearchValueArray.Where(str => int.TryParse(str, out x)).Select(str => x).ToList();
                    break;
                case bool when propertyType == typeof(string):
                    expressionValue.Method = MethodContains.MakeGenericMethod(typeof(string));
                    break;
                default:
                    break;
            }
            return expressionValue;
        }
    }
    public class ExpressionValue
    {
        public MethodInfo Method { get; set; }
    }

    public static class ApplyOrdering
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return source;
            }
            var stringProperties = typeof(T).GetProperties();
            if (stringProperties.Any(x => string.Equals(x.Name, columnName, StringComparison.InvariantCultureIgnoreCase)))
            {
                ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

                MemberExpression property = Expression.Property(parameter, columnName);
                LambdaExpression lambda = Expression.Lambda(property, parameter);

                string methodName = isAscending ? "OrderBy" : "OrderByDescending";

                Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                      new Type[] { source.ElementType, property.Type },
                                      source.Expression, Expression.Quote(lambda));

                return source.Provider.CreateQuery<T>(methodCallExpression);
            }
            else
            {
                return source;
            }
        }
    }
}
