using ADO.Lite.Enums;
using ADO.Lite.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ADO.Lite.Common
{
    public static class ExtensionUtilities
    {

        public static void SetParameters(this IDbCommand command, SqlAndParameters sqlParameter)
        {
            // check befor use
            sqlParameter?.Parameter?.ForEach(keyValueParameter =>
            {
                IDbDataParameter parameters = command.CreateParameter();

                parameters.ParameterName = keyValueParameter.ParameterKey;
                parameters.Value = keyValueParameter.ParameterValue;

                command.Parameters.Add(parameters);
            });
        }

        public static void SetParameters<T>(this IDbCommand command, T dataValueObject, DbProvider DbProvider, string condition, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            string TableName = dataValueObject.GetType().Name;
            List<string> listColumn = new List<string>();
            string UpdateSet = string.Empty;

            UpdateSet = dataValueObject.GetParamTypeUpdateSet(DbProvider, excludeProperties);

            // set sql
            command.CommandText = $"update {TableName} set {UpdateSet} where {condition} ";

            // set command type
            command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            // get properties to save to db
            listColumn = dataValueObject.GetParamListName(excludeProperties);

            // set parameters
            listColumn.ForEach(keyValueParameter =>
            {
                IDbDataParameter parameters = command.CreateParameter();

                object ParameterValue = dataValueObject.GetType().GetProperty(keyValueParameter).GetValue(dataValueObject, null);

                parameters.ParameterName = keyValueParameter.GetParamType(DbProvider);
                parameters.Value = ParameterValue;

                command.Parameters.Add(parameters);
            });
        }

        private static string GetParamTypeUpdateSet<T>(this T dataValueObject, DbProvider DbProvider, List<string> excludeProperties = null)
        {
            string paramType = string.Empty;

            List<string> listColumn = new List<string>();
            List<string> listColumnout = new List<string>();

            listColumn = dataValueObject.GetParamListName(excludeProperties);

            listColumn.ForEach(ParamName =>
            {
                if (DbProvider == DbProvider.SqlClient || DbProvider == DbProvider.MySqlClient)
                {
                    paramType = "@" + ParamName;
                }
                else if (DbProvider == DbProvider.DB2Client)
                {
                    paramType = "?";
                }

                listColumnout.Add(ParamName + "=" + paramType);
            });

            return string.Join(",", listColumnout);
        }

        public static void SetParameters<T>(this IDbCommand command, T dataValueObject, DbProvider DbProvider, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            string TableName = dataValueObject.GetType().Name;
            string propertyFieldNames = string.Empty;
            string paramsFieldKeys = string.Empty;
            List<string> listColumn = new List<string>();

            propertyFieldNames = dataValueObject.GetPropertyNames(excludeProperties);
            paramsFieldKeys = dataValueObject.GetParamTypeString(DbProvider, excludeProperties);

            // set sql
            command.CommandText = $"insert into {TableName} ({propertyFieldNames}) values({paramsFieldKeys}) ";

            // set command type
            command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            // get properties to save to db
            listColumn = dataValueObject.GetParamListName(excludeProperties);

            // set parameters
            listColumn.ForEach(keyValueParameter =>
            {
                IDbDataParameter parameters = command.CreateParameter();

                object ParameterValue = dataValueObject.GetType().GetProperty(keyValueParameter).GetValue(dataValueObject, null);

                parameters.ParameterName = keyValueParameter.GetParamType(DbProvider);
                parameters.Value = ParameterValue;

                command.Parameters.Add(parameters);
            });
        }

        public static string GetStringFromExpression<T>(this Expression<Func<T, bool>> expression)
        {
            var replacements = new Dictionary<string, string>();
            string bodyCondition = string.Empty;
            WalkExpression(replacements, expression);

            string body = expression.Body.ToString();

            foreach (var parm in expression.Parameters)
            {
                var parmName = parm.Name;
                var parmTypeName = parm.Type.Name;
                body = body.Replace(parmName + ".", parmTypeName + ".");
            }

            foreach (var replacement in replacements)
            {
                body = body.Replace(replacement.Key, replacement.Value);
            }

            bodyCondition = ReplaceWithLike(body).Replace(typeof(T).Name + ".", "");

            return bodyCondition;
        }

        private static string ReplaceWithLike(string sql)
        {
            string sqlCondition = string.Empty;

            sqlCondition = ReplaceContains(sql).ReplaceEndsWith().ReplaceStartsWith();

            return sqlCondition;
        }

        private static string ReplaceContains(string sql)
        {
            string sqlCondition = sql;

            sqlCondition = sql.Replace("==", "=").
                                Replace("AndAlso", " And ").
                                Replace("OrElse", " or ").
                                Replace(@"\", "").
                                Replace(@"""", "'");

            if (sqlCondition.Contains(".Contains") || sqlCondition.Contains(".EndsWith") || sqlCondition.Contains(".StartsWith"))
            {
                sqlCondition = sqlCondition.Replace(".Contains('", " like ('%");

                string[] Values = sqlCondition.Split(new char[] { '%', ')' });

                var ValuesContain = Values.Where(x => string.IsNullOrEmpty(x) == false && x.Contains("like") == false && x.Contains("EndsWith") == false && x.Contains("StartsWith") == false).ToList();

                ValuesContain.ForEach((y) => { sqlCondition = sqlCondition.Replace(y, y.Replace("'", "") + "%'"); });
            }
            return sqlCondition;
        }

        private static string ReplaceEndsWith(this string sqlCondition)
        {
            if (sqlCondition.Contains(".Contains") || sqlCondition.Contains(".EndsWith") || sqlCondition.Contains(".StartsWith"))
            {
                string[] Values = sqlCondition.Split(new char[] { '%', ')' });

                var ValuesEndsWith = Values.Where(x => string.IsNullOrEmpty(x) == false && x.Contains("like") == false && x.Contains("EndsWith") == true).ToList();

                ValuesEndsWith?.ForEach((y) =>
                {
                    var EndWithValues = y.Split(new string[] { ".EndsWith" }, StringSplitOptions.None);

                    sqlCondition = sqlCondition.Replace(y, EndWithValues[0] + " like ('" + EndWithValues[1].Replace("'", "").Replace("(", "") + "%'");
                });
            }

            return sqlCondition;
        }

        private static string ReplaceStartsWith(this string sqlCondition)
        {
            if (sqlCondition.Contains(".Contains") || sqlCondition.Contains(".EndsWith") || sqlCondition.Contains(".StartsWith"))
            {
                sqlCondition = sqlCondition.Replace(".StartsWith('", " like ('%").Replace("(", "").Replace(")", "");
            }

            return sqlCondition;
        }

        private static void WalkExpression(Dictionary<string, string> replacements, Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    string replacementExpression = expression.ToString();
                    if (replacementExpression.Contains("value("))
                    {
                        string replacementValue = Expression.Lambda(expression).Compile().DynamicInvoke().ToString();
                        if (!replacements.ContainsKey(replacementExpression))
                        {
                            replacements.Add(replacementExpression, replacementValue.ToString());
                        }
                    }
                    break;

                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                case ExpressionType.Equal:
                    var bexp = expression as BinaryExpression;
                    WalkExpression(replacements, bexp.Left);
                    WalkExpression(replacements, bexp.Right);
                    break;

                case ExpressionType.Call:
                    var mcexp = expression as MethodCallExpression;
                    foreach (var argument in mcexp.Arguments)
                    {
                        WalkExpression(replacements, argument);
                    }
                    break;

                case ExpressionType.Lambda:
                    var lexp = expression as LambdaExpression;
                    WalkExpression(replacements, lexp.Body);
                    break;

                case ExpressionType.Constant:
                    //do nothing
                    break;

                default:
                    //Trace.WriteLine("Unknown type");
                    break;
            }
        }

        public static List<string> GetListStringFromExpression<T>(this Expression<Func<T, bool>> expression)
        {
            var replacements = new Dictionary<string, string>();
            List<string> bodyCondition = new List<string>();
            WalkExpression(replacements, expression);
            List<string> propertyList = new List<string>();

            string body = expression.Body.ToString();

            propertyList = typeof(T).GetProperties().Select(x => x.Name).ToList();

            propertyList.ForEach(p => { if (body.Contains(p)) bodyCondition.Add(p); });


            return bodyCondition;
        }

        private static string GetParamType(this string ParamName, DbProvider DbProvider)
        {
            string paramType = string.Empty;

            if (DbProvider == DbProvider.SqlClient || DbProvider == DbProvider.MySqlClient)
            {
                paramType = "@" + ParamName;
            }
            else if (DbProvider == DbProvider.DB2Client)
            {
                paramType = "?";
            }

            return paramType;
        }

        private static string GetParamTypeString<T>(this T dataValueObject, DbProvider DbProvider, List<string> excludeProperties = null)
        {
            string paramType = string.Empty;
            List<string> listColumn = new List<string>();

            listColumn = dataValueObject.GetParamListName(excludeProperties);

            paramType = listColumn.GetParamTypeString(DbProvider);

            return paramType;
        }

        private static string GetParamTypeString(this List<string> paramList, DbProvider DbProvider)
        {
            string paramName = string.Empty;
            List<string> listColumnout = new List<string>();

            paramList.ForEach(ParamName =>
            {
                if (DbProvider == DbProvider.SqlClient || DbProvider == DbProvider.MySqlClient)
                {
                    paramName = "@" + ParamName;
                }
                else if (DbProvider == DbProvider.DB2Client)
                {
                    paramName = "?";
                }

                listColumnout.Add(paramName);
            });

            return string.Join(",", listColumnout);
        }

        private static List<string> GetParamListName<T>(this T dataValueObject, List<string> excludeProperties = null)
        {
            List<string> listColumn = new List<string>();

            if (excludeProperties != null)
            {
                listColumn = dataValueObject.GetType().GetProperties().Where(u => !excludeProperties.Exists(p => p.Equals(u.Name, StringComparison.OrdinalIgnoreCase))).Select(x => x.Name).ToList();
            }
            else
            {
                listColumn = dataValueObject.GetType().GetProperties().Select(x => x.Name).ToList();
            }

            return listColumn;
        }

        public static string GetPropertyNames<T>(this T dataObject, List<string> excludeProperties = null)
        {
            List<string> listColumn = new List<string>();

            if (excludeProperties != null)
            {
                listColumn = dataObject.GetType().GetProperties().Where(u => !excludeProperties.Exists(p => p.Equals(u.Name, StringComparison.OrdinalIgnoreCase))).Select(x => "[" + x.Name + "]").ToList();
            }
            else
            {
                listColumn = dataObject.GetType().GetProperties().Select(x => "[" + x.Name + "]").ToList();
            }

            return string.Join(",", listColumn);
        }
    }
}