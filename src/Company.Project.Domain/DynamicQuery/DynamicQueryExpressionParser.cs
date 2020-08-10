using Riven.Specifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Company.Project.DynamicQuery
{
    /// <summary>
    /// 动态查询表达式转换器
    /// </summary>
    public static class DynamicQueryExpressionParser<T>
    {

        public static ParameterExpression Parameter { get; private set; }
        public static char SplitChar { get; private set; }
        public static char[] SplitChars { get; private set; }

        #region Types

        public static Type CharType { get; private set; }
        public static Type StringType { get; private set; }
        public static Type GuidType { get; private set; }
        public static Type GuidOrNullType { get; private set; }
        public static Type LongType { get; private set; }
        public static Type LongOrNullType { get; private set; }
        public static Type Int32Type { get; private set; }
        public static Type Int32OrNullType { get; private set; }
        public static Type DoubleType { get; private set; }
        public static Type DoubleOrNullType { get; private set; }
        public static Type FloatType { get; private set; }
        public static Type FloatOrNullType { get; private set; }
        public static Type DecimalType { get; private set; }
        public static Type DecimalOrNullType { get; private set; }
        public static Type BoolType { get; private set; }
        public static Type BoolOrNullType { get; private set; }
        public static Type DateTimeType { get; private set; }
        public static Type DateTimeOrNullType { get; private set; }


        #endregion

        #region MethodInfos

        public static MethodInfo StartsWithMethod { get; private set; }

        #endregion

        static DynamicQueryExpressionParser()
        {
            Parameter = Expression.Parameter(typeof(T), "p");
            SplitChar = '.';
            SplitChars = new char[] { SplitChar };

            #region Types

            CharType = typeof(char);
            StringType = typeof(string);

            GuidType = typeof(Guid);
            GuidOrNullType = typeof(Guid?);

            LongType = typeof(long);
            LongOrNullType = typeof(long?);


            Int32Type = typeof(int);
            Int32OrNullType = typeof(int?);


            DoubleType = typeof(double);
            DoubleOrNullType = typeof(double?);


            FloatType = typeof(float);
            FloatOrNullType = typeof(float?);

            DecimalType = typeof(decimal);
            DecimalOrNullType = typeof(decimal?);

            BoolType = typeof(bool);
            BoolOrNullType = typeof(bool?);

            DateTimeType = typeof(DateTime);
            DateTimeOrNullType = typeof(DateTime?);

            #endregion


            #region MethodInfos

            StartsWithMethod = StringType.GetMethod("StartsWith", new[] { StringType });

            #endregion

        }


        public static Expression<Func<T, bool>> ParseExpressionBody(IEnumerable<QueryCondition> conditions)
        {
            Expression<Func<T, bool>> result = null;
            foreach (var condition in conditions)
            {
                //查询条件值不为空,才加入查询条件
                if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    Expression<Func<T, bool>> tempCondition = ParseCondition(condition);

                    if (result == null)
                    {
                        result = tempCondition;
                    }
                    else
                    {
                        result = ExpressionFuncExtender.And(result, tempCondition);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 根据查询条件生产LambdaExpression表达式
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>Expression</returns>
        static Expression<Func<T, bool>> ParseCondition(QueryCondition condition)
        {
            var field = condition.Field;
            var fieldValue = condition.Value;
            var queryOperator = condition.Operator;

            var fieldExpression = GetFieldExpression(condition.Field);

            var fieldType = fieldExpression.Type;

            var constantExpressions = GetConstantExpressions(field, fieldValue, fieldType, queryOperator);

        }


        /// <summary>
        /// 获取字段表达式
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        static Expression GetFieldExpression(string field)
        {
            if (field.Contains(SplitChar))
            {
                var field1 = field.Split(SplitChars)[0];
                var field2 = field.Split(SplitChars)[1];
                MemberExpression parentMember = Expression.PropertyOrField(Parameter, field1);

                return Property(parentMember, field2);
            }

            return Property(Parameter, field);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="queryOperator">查询类型</param>
        /// <returns></returns>
        static (ConstantExpression a, ConstantExpression b) GetConstantExpressions(string field, string fieldValue, Type fieldType, QueryOperatorType queryOperator)
        {

            var constantExpression1 = default(ConstantExpression);
            var constantExpression2 = default(ConstantExpression);


            if (fieldType == StringType)
            {
                if (!fieldValue.Contains(","))
                {
                    constantExpression1 = Expression.Constant(fieldValue, fieldType);
                }
            }
            else if (fieldType == LongType || fieldType == LongOrNullType)
            {
                if (!fieldValue.Contains(","))
                {
                    var longValue = long.Parse(fieldValue);
                    constantExpression1 = Expression.Constant(longValue, fieldType);
                }

            }
            else if (fieldType == Int32Type || fieldType == Int32OrNullType)
            {
                if (!fieldValue.Contains(","))
                {
                    var Int32Value = int.Parse(fieldValue);
                    constantExpression1 = Expression.Constant(Int32Value, fieldType);
                }
            }
            else if (fieldType == DoubleType || fieldType == DoubleOrNullType)
            {
                var doubleValue = double.Parse(fieldValue);
                constantExpression1 = Expression.Constant(doubleValue, fieldType);
            }
            else if (fieldType == FloatType || fieldType == FloatOrNullType)
            {
                var floatValue = float.Parse(fieldValue);
                constantExpression1 = Expression.Constant(floatValue, fieldType);
            }
            else if (fieldType == DateTimeType || fieldType == DateTimeOrNullType)
            {
                if (queryOperator == QueryOperatorType.Between)
                {

                    string[] strDate = fieldValue.Split("|".ToCharArray());
                    DateTime startDT = DateTime.Parse(strDate[0]);
                    DateTime endDT = DateTime.Parse(strDate[1]);
                    constantExpression1 = Expression.Constant(startDT, fieldType);
                    constantExpression2 = Expression.Constant(endDT, fieldType);
                }
                else
                {
                    var dateTimeValue = DateTime.Parse(fieldValue);
                    constantExpression1 = Expression.Constant(dateTimeValue, fieldType);
                }
            }
            else if (fieldType == BoolType || fieldType == BoolOrNullType)
            {
                var boolValue = fieldValue == "1" ? true : false;
                constantExpression1 = Expression.Constant(boolValue, fieldType);
            }


            return (constantExpression1, constantExpression2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldNotNull"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        static Expression<Func<T, bool>> GetExpressionWithQueryOperator(string field, string fieldValue, Type fieldType, bool fieldNotNull, QueryOperatorType queryOperator, Expression fieldExpression)
        {
            switch (queryOperator)
            {
                case QueryOperatorType.StartsWith:
                    if (fieldNotNull)
                    {
                        var methodCallExpression = Expression.Call(
                                fieldExpression,
                                StartsWithMethod,
                                Expression.Convert(fieldExpression, fieldType)
                            );

                        return Expression.Lambda<Func<T, bool>>(methodCallExpression, Parameter);
                    }

                    var notNullExp = Expression.NotEqual(fieldExpression, Expression.Constant(null, fieldType));
                    var predicate = Expression.Lambda<Func<T, bool>>(notNullExp, Parameter);
                    break;
                case QueryOperatorType.EndsWith:
                    break;
                case QueryOperatorType.Equal:
                    break;
                case QueryOperatorType.NotEqual:
                    break;
                case QueryOperatorType.Greater:
                    break;
                case QueryOperatorType.GreaterEqual:
                    break;
                case QueryOperatorType.Less:
                    break;
                case QueryOperatorType.LessEqual:
                    break;
                case QueryOperatorType.Between:
                    break;
                case QueryOperatorType.In:
                    break;
                case QueryOperatorType.Contains:
                    break;
                default:
                    break;
            }
        }

        static Expression Property(Expression target, string field)
        {
            return field.Split('.').Aggregate(target, SimpleProperty);
        }


        static Expression SimpleProperty(Expression target, string field)
        {
            return Expression.MakeMemberAccess(target, GetProperty(target.Type, field));
        }

        static PropertyInfo GetProperty(Type type, string field)
        {
            while (type != null)
            {
                var property = type.GetProperty(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    return property;
                }

                type = type.BaseType;
            }


            return null;
        }
    }
}
