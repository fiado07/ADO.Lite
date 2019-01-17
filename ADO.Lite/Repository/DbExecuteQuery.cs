using ADO.Lite.Common;
using ADO.Lite.Contracts;
using ADO.Lite.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ADO.Lite.Repository
{
    /// <summary>
    /// Execute Query
    /// </summary>
    /// <seealso cref="ORM.lite.ADO.Contracts.IDbExecute" />
    public class DbExecuteQuery : Contracts.IDbExecute
    {
        private IDbConnectionSql DbConnection;

        #region constutor

        public DbExecuteQuery(IDbConnectionSql dbConnection)
        {
            DbConnection = dbConnection;
        }

        #endregion constutor

        #region methods

        private void isConnectionStringValid()
        {
            if (DbConnection?.DbConnectionBase?.ConnectionString == null)
                throw new Exception("Object connection or connectionString is null.");

            if (DbConnection.DbConnectionBase.State == ConnectionState.Closed)
                DbConnection.DbConnectionBase.Open();
        }

        /// <summary>
        /// Check if is there any data
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public bool any(SqlAndParameters sqlParameter)
        {
            bool result = false;
            IDataReader readerResult = null;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get and set values
                    readerResult = sqlComand.ExecuteReader();

                    while (readerResult.Read())
                    {
                        if (readerResult[0] != null) result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();

                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return result;
        }

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public void ExecuteSql(SqlAndParameters sqlParameter)
        {
            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql

                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // execute query
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose)
                    DbConnection.DbConnectionBase.Close();
            }
        }

        /// <summary>
        /// Executes the SQL get object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public T ExecuteSqlGetObject<T>(SqlAndParameters sqlParameter) where T : new()
        {
            T objectType = new T();
            IDataReader readerResult = null;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand?.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get and set values
                    readerResult = sqlComand.ExecuteReader(CommandBehavior.SingleRow);

                    // map data from reader to new object
                    objectType = Mappers.Mapper.Map<T>(readerResult).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return objectType;
        }

        /// <summary>
        /// Executes the SQL get object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public IEnumerable<T> ExecuteSqlGetObjectList<T>(SqlAndParameters sqlParameter) where T : new()
        {
            IEnumerable<T> objectTypeList;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get values from reader
                    IDataReader readerResult = sqlComand.ExecuteReader();

                    // get list of object from reader
                    objectTypeList = Mappers.Mapper.Map<T>(readerResult);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return objectTypeList;
        }

        /// <summary>
        /// Executes the SQL get single.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Object connection or connectionString is null.</exception>
        public object ExecuteSqlGetSingle(SqlAndParameters sqlParameter)
        {
            object total = null;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get values from reader
                    var totalScalar = sqlComand.ExecuteScalar();

                    // get list of object from reader
                    total = Convert.IsDBNull(totalScalar) ? total : totalScalar;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return total;
        }

        /// <summary>
        /// Executes the SQL get tabela.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="DbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Object connection or connectionString is null.
        /// or
        /// Object SetAdapter is null.
        /// </exception>
        public DataTable ExecuteSqlGetTabela(SqlAndParameters sqlParameter)
        {
            DataSet dstable = new DataSet();
            IDbCommand sqlComand;

            try
            {
                isConnectionStringValid();

                if (DbConnection?.SetAdapter == null)
                    throw new Exception("Object SetAdapter can not be null, initialize!");

                // initialize sqlcommand
                sqlComand = DbConnection.DbConnectionBase.CreateCommand();

                if (DbConnection?.DbTransaction != null)
                    sqlComand.Transaction = DbConnection.DbTransaction;

                if (sqlComand.Parameters != null)
                    sqlComand.SetParameters(sqlParameter);

                // set sql
                sqlComand.CommandText = sqlParameter.Sql;

                // set command type
                sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                // set command on adapter
                DbConnection.SetAdapter.SelectCommand = sqlComand;

                // set get data
                DbConnection.SetAdapter.Fill(dstable);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return dstable.Tables[0];
        }

        public void Add<T>(T dataObject, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            string propertyFieldNames = string.Empty;
            string paramsFieldKeys = string.Empty;

            List<Parameter> parameters = new List<Parameter>();

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    // set parameters and sql
                    sqlComand.SetParameters(dataObject, DbConnection.DbProvider, excludeProperties);

                    // execute
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        public void Update<T>(T dataObject, string predicate, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    // set parameters and sql
                    sqlComand.SetParameters(dataObject, DbConnection.DbProvider, predicate, excludeProperties);

                    // execute
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                    // set parameters and sql
                    sqlComand.CommandText = $"delete from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            T entity = new T();

            try
            {
                entity = GetList(predicate).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            IEnumerable<T> entityList = Enumerable.Empty<T>();
            T entity = new T();
            List<Parameter> parameters = new List<Parameter>();
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                    // set parameters and sql
                    sqlComand.CommandText = $"select * from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute and get list
                    entityList = Mappers.Mapper.Map<T>(sqlComand.ExecuteReader()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return entityList;
        }

        public bool any<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            T entity = new T();
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;
            bool any = false;

            try
            {
                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                    //fieldNames = entity.GetPropertyNames();

                    // set parameters and sql
                    sqlComand.CommandText = $"select * from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute and get list
                    any = sqlComand.ExecuteReader().Read();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return any;
        }

        /// <summary></summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <param name="predicate"></param>
        /// <param name="excludeProperties"></param>
        /// <param name="isStoredProcedure"></param>
        public void Update<T>(T dataObject, Expression<Func<T, bool>> predicate, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            try
            {


                isConnectionStringValid();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    string localPredicate = predicate.GetStringFromExpression();

                    if (excludeProperties == null)
                    {
                        excludeProperties = predicate.GetListStringFromExpression();
                    }
                    else
                    {
                        predicate.GetListStringFromExpression().
                            ForEach(item =>
                                            {
                                                if (!excludeProperties.Contains(item)) excludeProperties.Add(item);
                                            });
                    }

                    // set parameters and sql
                    sqlComand.SetParameters(dataObject, DbConnection.DbProvider, localPredicate, excludeProperties);

                    // execute
                    sqlComand.ExecuteNonQuery();


                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        #endregion methods
    }
}