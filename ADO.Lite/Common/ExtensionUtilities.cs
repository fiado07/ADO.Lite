using ADO.Lite.Enums;
using ADO.Lite.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ADO.Lite.Common
{
    public static class ExtensionUtilities
    {

        public static Dictionary<string, string> dbTypes = null;

        public static Dictionary<string, string> GetdbTypes(this IDbCommand sqlCmd, string nomeTabela, DbProvider dbProvider = DbProvider.SqlClient)
        {

            Dictionary<string, string> dictionaryDbTypes = new Dictionary<string, string>();
            string sql = string.Empty;


            try
            {


                if (dbProvider == DbProvider.SqlClient)
                {

                    sql = string.Format("SELECT DATA_TYPE,COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = '{0}' ", nomeTabela);

                }
                else if (dbProvider == DbProvider.DB2Client)
                {
                    sql = string.Format("SELECT COLTYPE DATA_TYPE, NAME COLUMN_NAME FROM SYSIBM.SYSCOLUMNS WHERE  TBNAME = '{0}' ", nomeTabela);

                }
                else if (dbProvider == DbProvider.MySqlClient)
                {
                    sql = string.Format("SELECT DATA_TYPE, COLUMN_NAME FROM SYSIBM.SYSCOLUMNS WHERE  TABLE_NAME = '{0}' ", nomeTabela);

                }


                sqlCmd.CommandText = sql;

                //if (sqlCmd.Connection.)

                using (var reader = sqlCmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        dictionaryDbTypes.Add(reader["COLUMN_NAME"].ToString(), reader["DATA_TYPE"].ToString());

                    }

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dictionaryDbTypes;

        }

        public static string GetTableName(this string inSql)
        {
            return inSql.ToLower().Split(new string[] { "from", "into", "update" }, StringSplitOptions.None)[1].Trim().ToString().Split(' ')[0].ToString();

        }

        public static void SetParameters(this IDbCommand command, SqlParameter sqlParameter)
        {

            string tableName = sqlParameter.Sql.GetTableName();

            // get db types
            dbTypes = command.GetdbTypes(nomeTabela: tableName);


            // check befor use
            sqlParameter?.Parameter?.ForEach(keyValueParameter =>
            {

                IDbDataParameter parameters = command.CreateParameter();
                string dbDataType = dbTypes.FirstOrDefault(p => "@" + p.Key == keyValueParameter.Key).Value.Trim();

                parameters.ParameterName = keyValueParameter.Key;
                parameters.Value = keyValueParameter.Value;



                if (dbDataType == "date")
                {
                    parameters.DbType = DbType.Date;
                    parameters.Value = DateTime.Parse(keyValueParameter.Value);
                }
                else if (dbDataType == "datetime")
                {
                    parameters.DbType = DbType.DateTime;
                    parameters.Value = DateTime.Parse(keyValueParameter.Value);

                }
                else if (dbDataType == "int")
                {
                    parameters.DbType = DbType.Int32;
                    parameters.Value = int.Parse(keyValueParameter.Value);

                }
                else if (dbDataType == "decimal")
                {
                    parameters.DbType = DbType.Decimal;
                    parameters.Value = Decimal.Parse(keyValueParameter.Value);

                }
                else if (dbDataType == "Double")
                {
                    parameters.DbType = DbType.Double;
                    parameters.Value = Double.Parse(keyValueParameter.Value);
                }
                else if (dbDataType.EndsWith("char"))
                {

                    parameters.DbType = DbType.String;
                    parameters.Value = keyValueParameter.Value;

                }
                command.Parameters.Add(parameters);


            });


        }


        public static Dictionary<string, string> GetdbTypes(this IDbCommand sqlCmd)
        {


            try
            {
                // get db types
                return dbTypes;

            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// Gets the property name list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The in object.</param>
        /// <returns></returns>
        public static List<string> GetPropertyNameList<T>(this T inObject)
        {
            return inObject.GetType().GetProperties().Cast<System.Reflection.PropertyInfo>().Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Transform object proprietes to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The in object.</param>
        /// <returns></returns>
        public static List<KeyValue> GetPropertyValueList<T>(this T inObject)
        {
            List<KeyValue> _propertyValueList = new List<KeyValue>();
            KeyValue _keyValue;
            Type _delegateFuncType = typeof(Func<T, Object>);
            //Func<T, Object> _getValor;


            // get lists of properties

            inObject.GetType().GetProperties().ToList().
                                            ForEach((_propertyInfo) =>
                                            {
                                                _keyValue = new KeyValue();
                                                // Set func
                                                //_getValor = (Func<T, Object>)Delegate.CreateDelegate(_delegateFuncType,null, _propertyInfo.GetGetMethod());

                                                _keyValue.Key = _propertyInfo.Name;
                                                _keyValue.Value = Convert.ToString(_propertyInfo.GetValue(inObject, null));
                                                //_keyValue.Value = (string)_getValor.Invoke(inObject);

                                                //  set property and propertyValue
                                                _propertyValueList.Add(_keyValue);

                                            });


            return _propertyValueList;

        }



        /// <summary>
        /// Gets the property parameter list. Ex: a=@a
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The in object.</param>
        /// <returns></returns>
        public static List<KeyValue> GetPropertyParamList<T>(this T inObject)
        {
            List<KeyValue> _propertyValueList = new List<KeyValue>();
            KeyValue _keyValue;
            //Type _delegateFuncType = typeof(Func<T, Object>);


            // get lists of properties

            inObject.GetType().GetProperties().ToList().
                                            ForEach((_propertyInfo) =>
                                            {
                                                _keyValue = new KeyValue();
                                                // Set func
                                                //_getValor = (Func<T, Object>)Delegate.CreateDelegate(_delegateFuncType,null, _propertyInfo.GetGetMethod());

                                                _keyValue.Key = _propertyInfo.Name;
                                                _keyValue.Value = "@" + _propertyInfo.Name;

                                                //  set property and propertyValue
                                                _propertyValueList.Add(_keyValue);

                                            });


            return _propertyValueList;

        }




    }
}
