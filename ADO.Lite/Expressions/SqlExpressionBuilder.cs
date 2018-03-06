

using ADO.Lite.Common;
using ADO.Lite.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Lite.Expressions
{
    public static class SqlExpressionBuilder
    {


        /// <summary>
        /// Builds the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> buildPredicate<T>(this T objectType, ExpressionType nodeType)
        {

            Expression<Func<T, Boolean>> _Expression = null;
            Expression<Func<T, Boolean>> _ExpressionResult = null;
            PropertyInfo _property = null;
            ParameterExpression _parameter = null;
            MemberExpression _memberAccess = null;
            ConstantExpression _constante = null;

            Type _delegateFuncType = typeof(Func<T, Object>);


            try
            {

                if (objectType == null) { return null; }


                Array.ForEach(objectType.GetType().GetProperties(),
                    (_propInfo) =>
                    {

                        // Set func
                        //_getValor = (Func<T, Object>)Delegate.CreateDelegate(_delegateFuncType, null, _propInfo.GetGetMethod());
                        //var _value = _getValor.Invoke(objectType);

                        object _value = _propInfo.GetValue(objectType, null);

                        if (_propInfo.Name.StartsWith("Entity")) { return; }


                        if (!string.IsNullOrEmpty(_value.ToString()))
                        {

                            //' -- :: get property Name
                            _property = objectType.GetType().GetProperty(_propInfo.Name);

                            //' -- :: get parameter
                            _parameter = Expression.Parameter(objectType.GetType(), "i");


                            //' --:: definicao da constante [ valor a ser pesquisado na DB ]:: --

                            _constante = Expression.Constant(_value);


                            //' -- :: definição do menbro [ i.Campo ]:: --

                            _memberAccess = Expression.MakeMemberAccess(_parameter, _property);


                            //var value = Expression.Constant(fieldValue);
                            var property = Expression.Property(_parameter, _property.Name);
                            var value = Expression.Constant(_value);
                            var converted = Expression.Convert(value, property.Type);



                            //' -- :: combine parts [Operador logico de juncao]
                            //var _binary = Expression.MakeBinary(ExpressionType.Equal, _memberAccess, _constante);
                            var comparison = Expression.Equal(property, converted);


                            //' -- :: create a lambda expression :: --

                            _Expression = Expression.Lambda<Func<T, Boolean>>(comparison, _parameter);


                            //' -- :: build condition

                            _ExpressionResult = ConditionBuilderCombine(_ExpressionResult, nodeType, _Expression);

                        }

                    });

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return _ExpressionResult;

        }

        /// <summary>
        /// Conditions the builder combine.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inPredicateLeft">The in predicate left.</param>
        /// <param name="inNode">The in node.</param>
        /// <param name="inPredicateRight">The in predicate right.</param>
        /// <returns></returns>
        private static Expression<Func<T, Boolean>> ConditionBuilderCombine<T>(this Expression<Func<T, Boolean>> inPredicateLeft,
                                        ExpressionType inNode,
                                        Expression<Func<T, Boolean>> inPredicateRight)
        {

            Expression<Func<T, Boolean>> _CombinedParts = null;
            InvocationExpression invokeParams = null;
            BinaryExpression JuntaParts = null;


            if (inPredicateLeft == null)
            {
                _CombinedParts = inPredicateRight;

            }
            else
            {

                //' -- :: Invoke parameters from expression :: --

                invokeParams = Expression.Invoke(inPredicateLeft, inPredicateRight.Parameters.Cast<Expression>());


                //' -- :: cria combinação/operacao logica :: --

                JuntaParts = Expression.MakeBinary(inNode, inPredicateRight.Body, invokeParams);


                //' -- :: combined parts [Criação de Expressão lambda] :: --

                _CombinedParts = Expression.Lambda<Func<T, Boolean>>(JuntaParts, inPredicateRight.Parameters);

            }

            return _CombinedParts;

        }

        /// <summary>
        /// SQLs the build select.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The in object.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="nomeTabela">The nome tabela.</param>
        /// <param name="outParams">The out parameters.</param>
        /// <returns></returns>
        internal static SqlParameter sqlBuildSelect<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "", string[] outParams = null)
        {

            string _nome = String.IsNullOrEmpty(nomeTabela) ? typeof(T).Name : nomeTabela;
            SqlParameter sqlparameter = new SqlParameter();
            List<KeyValue> list = new List<KeyValue>();
            string _predicate = string.Empty;


            // get predicate translated
            if (predicate != null)
            {
                sqlparameter = predicate.GetStringPredicate();
                _predicate = "where " + sqlparameter.Predicate;

            }

            // set sql 
            sqlparameter.Sql = String.Format("select {0} from {1} {2} ", outParams == null ? "*" : string.Join(",", outParams), _nome, _predicate);

            return sqlparameter;

        }


        /// <summary>
        /// SQLs the build select count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="nomeTabela">The nome tabela.</param>
        /// <returns></returns>
        internal static SqlParameter sqlBuildSelectCount<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {

            string _nome = String.IsNullOrEmpty(nomeTabela) ? typeof(T).Name : nomeTabela;
            SqlParameter sqlparameter = new SqlParameter();
            List<KeyValue> list = new List<KeyValue>();
            string _predicate = string.Empty;


            // get predicate translated
            if (predicate != null)
            {
                sqlparameter = predicate.GetStringPredicate();
                _predicate = "where " + sqlparameter.Predicate;

            }

            // set sql 
            sqlparameter.Sql = String.Format("select count(*) total from {0} {1} ", _nome, _predicate);

            return sqlparameter;

        }


        internal static SqlParameter sqlBuildSelectCountAll(this string nomeTabela)
        {

            SqlParameter sqlparameter = new SqlParameter();


            // set sql 
            sqlparameter.Sql = String.Format("select count(*) total from {0} ", nomeTabela);

            return sqlparameter;

        }


        /// <summary>
        /// SQLs the build update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The in object.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="nomeTabela">The nome tabela.</param>
        /// <param name="columnsToFilter">The columns to filter.</param>
        /// <returns></returns>
        internal static SqlParameter sqlBuildUpdate<T>(this T inObject, Expression<Func<T, Boolean>> predicate, string nomeTabela = "", List<string> columnsToFilter = null)
        {

            string _nome = String.IsNullOrEmpty(nomeTabela) ? typeof(T).Name : nomeTabela;
            SqlParameter sqlparameter;
            List<KeyValue> list = new List<KeyValue>();
            List<KeyValue> listValues = new List<KeyValue>();
            List<string> Updatelist = new List<string>();
            List<string> UpdatelistToRemove = new List<string>();

            // add parameters Ex: a=@a
            inObject.GetPropertyParamList<T>().ForEach(x => { list.Add(new KeyValue { Key = x.Key, Value = x.Value }); });


            // filter list of keys in case of restriction
            if (columnsToFilter != null)
            {
                columnsToFilter.ForEach(_listParam =>
                {

                    var _result = list.Find(x => x.Key.Trim() == _listParam.Trim());

                    list.Remove(_result);

                });
            }


            // get properties values ::
            listValues = inObject.GetPropertyValueList().Select(_values => new KeyValue { Key = "@" + _values.Key.Trim(), Value = _values.Value }).ToList();


            // get sql predicate
            sqlparameter = predicate.GetStringPredicate();


            // Check list of values to Update
            sqlparameter.Parameter.ForEach(_param => list.Remove(new KeyValue { Key = _param.Key.Remove(0, 1).Trim(), Value = _param.Key.Trim() }));


            // Combine values Ex: a=@a
            list.ForEach(param => Updatelist.Add(param.Key + " = " + param.Value));


            // remove conditions

            Updatelist.ForEach(x =>
                                {

                                    if (sqlparameter.Predicate.Contains(x)) UpdatelistToRemove.Add(x);


                                });

            UpdatelistToRemove.ForEach(x => Updatelist.Remove(x));



            // update parameters :: add conditions to filter row to update
            listValues.ForEach(_params =>
            {

                if (list.Exists(x => x.Value == _params.Key) && !sqlparameter.Parameter.Exists(x => x.Key == _params.Key))
                {

                    sqlparameter.Parameter.Add(_params);

                }


            });


            // set sql 
            sqlparameter.Sql = String.Format("update {0} set {1} where {2} ", _nome, string.Join(",", Updatelist), sqlparameter.Predicate);

            return sqlparameter;


        }

        internal static SqlParameter sqlBuildInsert<T>(this T inObject, string nomeTabela = "", List<string> columnsToFilter = null)
        {

            string _name = String.IsNullOrEmpty(nomeTabela) ? typeof(T).Name : nomeTabela;
            List<string> _keysToExclude = new List<string>(); // for auto-increment/identity
            SqlParameter sqlparameter = new SqlParameter();
            List<KeyValue> _lisKeyValue = new List<KeyValue>();
            string sqlColumns = string.Empty;
            string sqlValues = string.Empty;

            sqlparameter.Parameter = new List<KeyValue>();


            // get keys

            _keysToExclude = inObject.GetType().GetProperties().
                                                Where(x => x.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).
                                                Select(x => x.Name).ToList();

            // add keys to filter
            if (columnsToFilter != null)
                columnsToFilter.ForEach(_item => _keysToExclude.Add(_item));



            // add parameters Ex: @a=2
            inObject.GetPropertyValueList().
                                            ForEach(x =>
                                            {
                                                if (!_keysToExclude.Exists(_key => _key.Trim() == x.Key.Trim()))
                                                    sqlparameter.Parameter.Add(new KeyValue { Key = "@" + x.Key, Value = x.Value });
                                            });


            // set property list and value Ex: @a,@b
            sqlparameter.Parameter.ForEach(x => { _lisKeyValue.Add(new KeyValue { Key = x.Key.Remove(0, 1), Value = x.Key }); });


            sqlColumns = GetInsertColumn(inObject, columnsToFilter);

            sqlValues = GetInsertColumnValues(_lisKeyValue);


            // set sql 
            sqlparameter.Sql = String.Format("insert into {0} ({1})  values ({2}) ", _name, sqlColumns, sqlValues);

            return sqlparameter;

        }

        private static string GetInsertColumnValues(List<KeyValue> inParamList, String delimiter = ",")
        {

            string _columnValues = string.Empty;


            // convert to string
            _columnValues = string.Join(delimiter, inParamList.Select(x => x.Value));

            return _columnValues;

        }

        private static string GetSelectColumnParameter(List<KeyValue> inParamList, String delimiter = ",")
        {

            string _columnValues = string.Empty;


            // convert to string
            _columnValues = string.Join(delimiter, inParamList);

            return _columnValues;

        }

        internal static string GetInsertColumn<T>(T inObject, List<string> excludeColumn)
        {

            string _column = string.Empty;
            List<string> _listColumn = new List<string>();
            List<PropertyInfo> _keysToExclude = new List<PropertyInfo>(); // for auto-increment/identity


            // get keys

            _keysToExclude = inObject.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList<PropertyInfo>();



            // add beaks []
            _listColumn = inObject.GetPropertyNameList().Select(x => "[" + x + "]").ToList();


            // filter keys
            if (_keysToExclude != null)
                _keysToExclude.ForEach(_info => _listColumn.Remove("[" + _info.Name + "]"));

            // filter keys
            if (excludeColumn != null)
                excludeColumn.ForEach(_info => _listColumn.Remove("[" + _info + "]"));

            // convert to string
            _column = string.Join(",", _listColumn);


            return _column;

        }

        internal static SqlParameter sqlBuidDelete<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {

            string _nome = String.IsNullOrEmpty(nomeTabela) ? typeof(T).Name : nomeTabela;
            SqlParameter sqlparameter;

            // get sql predicate
            sqlparameter = predicate.GetStringPredicate();

            // set sql 
            sqlparameter.Sql = String.Format("delete from {0} where {1} ", _nome, sqlparameter.Predicate);

            return sqlparameter;

        }

        /// <summary>
        /// Gets the string predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static SqlParameter GetStringPredicate<T>(this Expression<Func<T, Boolean>> predicate)
        {

            string _parameter = predicate.Parameters[0].Name + ".";
            string _predicate = string.Empty;
            string[] _splitCombination = { " AND ", " OR " };
            string[] _splitOperation = { "IS NOT", "IS", "LIKE", "=", "<>", "<", ">", "<=", ">=" };
            string[] _expressionCombination;
            string[] _expressionOperation;


            SqlParameter sqlparameter;
            List<KeyValue> KeyValueList = new List<KeyValue>();
            KeyValue _keyValue = null;


            // get predicate query

            _predicate = new SqlPredicateBuilder().Translate(predicate).Replace(_parameter, "").Replace("'", "");

            //get expression parts
            _expressionCombination = _predicate.Split(_splitCombination, StringSplitOptions.None);

            Array.ForEach(_expressionCombination, (_expression) =>
            {


                _expressionOperation = _expression.Split(_splitOperation, StringSplitOptions.None);

                var expressionReplace = _expression.Replace("(", "").Replace(")", "").Trim().Replace("'", "");
                var _key = _expressionOperation[0].Replace("(", "").Replace(")", "").Trim();
                var _value = _expressionOperation[1].Replace("(", "").Replace(")", "").Replace("'", "").Trim();

                // replace values by params

                _keyValue = new KeyValue();

                _keyValue.Key = "@" + _key;
                _keyValue.Value = _value;


                // add values to list
                KeyValueList.Add(_keyValue);


                string keyValueParameter = _key + expressionReplace.Replace(_key, "").Replace(_value, "") + _keyValue.Key;


                // update predicate with parameter:: --
                _predicate = _predicate.Replace(expressionReplace.Trim(), keyValueParameter);


            });


            // get plane sql and list of parameters
            sqlparameter = new SqlParameter() { Predicate = _predicate, Parameter = KeyValueList };


            return sqlparameter;

        }
                


    }
}
