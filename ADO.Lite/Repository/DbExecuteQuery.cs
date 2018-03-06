using System;
using System.Collections.Generic;
using System.Data;
using ADO.Lite.Contracts;
using ADO.Lite.Parameters;
using ADO.Lite.Common;

namespace ADO.Lite.Repository
{
    /// <summary>
    /// Execute Query
    /// </summary>
    /// <seealso cref="ORM.lite.ADO.Contracts.IDbExecute" />
    public class DbExecuteQuery : Contracts.IDbExecute
    {

        #region constutor

        public DbExecuteQuery()
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// Check if is there any data
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public bool any(SqlParameter sqlParameter, IDbConnectionSql dbConnection)
        {

            bool result = false;
            IDataReader readerResult = null;

            try
            {


                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");

                using (var sqlComand = dbConnection.DbConnectionBase.CreateCommand())
                {


                    if (sqlComand.Parameters != null)
                    {
                        sqlComand.SetParameters(sqlParameter);

                    }


                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;


                    // get and set values
                    readerResult = sqlComand.ExecuteReader();



                    while (readerResult.Read())
                    {

                        var fieldValue = readerResult[0];

                        if (fieldValue != null) result = true;

                    }


                }

            }
            catch (Exception ex)
            {

                throw;

            }
            finally
            {

                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();

                if (dbConnection.canClose) dbConnection.DbConnectionBase.Close();

            }

            
            return result;

        }

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public void ExecuteSql(SqlParameter sqlParameter, IDbConnectionSql dbConnection)
        {

            try
            {

                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");


                using (var sqlComand = dbConnection.DbConnectionBase.CreateCommand())
                {


                    if (sqlComand.Parameters != null)
                    {
                        sqlComand.SetParameters(sqlParameter);

                    }


                    // set sql 

                    sqlComand.CommandText = sqlParameter.Sql;


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

                if (dbConnection.canClose)
                    dbConnection.DbConnectionBase.Close();

            }




        }

        /// <summary>
        /// Executes the SQL get object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public T ExecuteSqlGetObject<T>(SqlParameter sqlParameter, IDbConnectionSql dbConnection) where T : new()
        {

            T objectType = new T();
            Dictionary<string, string> dbtypes = null;
            IDataReader readerResult = null;

            try
            {


                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");

                using (var sqlComand = dbConnection.DbConnectionBase.CreateCommand())
                {


                    if (sqlComand.Parameters != null)
                    {
                        sqlComand.SetParameters(sqlParameter);

                    }


                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;


                    // get and set values

                    readerResult = sqlComand.ExecuteReader(CommandBehavior.SingleRow);


                    // get dbTypes
                    dbtypes = sqlComand.GetdbTypes();


                    // map data from reader to new object
                    objectType = new Maper().Map<T>(readerResult, dbtypes);



                }

            }
            catch (Exception ex)
            {

                throw;

            }
            finally
            {

                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();
                if (dbConnection.canClose) dbConnection.DbConnectionBase.Close();
            }

            return objectType;

        }

        /// <summary>
        /// Executes the SQL get object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object connection or connectionString is null.</exception>
        public IEnumerable<T> ExecuteSqlGetObjectList<T>(SqlParameter sqlParameter, IDbConnectionSql dbConnection) where T : new()
        {

            IEnumerable<T> objectTypeList;
            Dictionary<string, string> dbtypes = null;

            try
            {


                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");

                using (var sqlComand = dbConnection.DbConnectionBase?.CreateCommand())
                {


                    if (sqlComand.Parameters != null)
                    {
                        sqlComand.SetParameters(sqlParameter);

                    }

                    // set sql 

                    sqlComand.CommandText = sqlParameter.Sql;


                    // get and set values

                    IDataReader readerResult = sqlComand.ExecuteReader();


                    // get db types

                    dbtypes = sqlComand.GetdbTypes();


                    // get data from reader to new object

                    objectTypeList = new Maper().MapToList<T>(readerResult, dbtypes);


                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                if (dbConnection.canClose) dbConnection.DbConnectionBase.Close();

            }


            return objectTypeList;


        }

        /// <summary>
        /// Executes the SQL get single.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Object connection or connectionString is null.</exception>
        public int ExecuteSqlGetSingle(SqlParameter sqlParameter, IDbConnectionSql dbConnection)
        {

            int total = 0;

            try
            {


                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");

                using (var sqlComand = dbConnection.DbConnectionBase?.CreateCommand())
                {


                    if (sqlComand.Parameters != null)
                    {
                        sqlComand.SetParameters(sqlParameter);

                    }

                    // set sql 

                    sqlComand.CommandText = sqlParameter.Sql;


                    // get scalar values

                    var totalScalar = sqlComand.ExecuteScalar();


                    // convert to int
                    total = Convert.IsDBNull(totalScalar) ? total : Convert.ToInt32(totalScalar);


                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                if (dbConnection.canClose) dbConnection.DbConnectionBase.Close();

            }


            return total;

            
        }

        /// <summary>
        /// Executes the SQL get tabela.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Object connection or connectionString is null.
        /// or
        /// Object SetAdapter is null.
        /// </exception>
        public DataTable ExecuteSqlGetTabela(SqlParameter sqlParameter, IDbConnectionSql dbConnection)
        {

            DataSet dstable = new DataSet();
            IDbCommand sqlComand;

            try
            {

                if (dbConnection?.DbConnectionBase?.ConnectionString == null)
                    throw new Exception("Object connection or connectionString is null.");

                if (dbConnection?.SetAdapter == null)
                    throw new Exception("Object SetAdapter is null, initialize!");


                // initialize sqlcommand
                sqlComand = dbConnection.DbConnectionBase.CreateCommand();


                if (sqlComand.Parameters != null)
                {
                    sqlComand.SetParameters(sqlParameter);

                }


                // set sql 

                sqlComand.CommandText = sqlParameter.Sql;


                // set command on adapter
                dbConnection.SetAdapter.SelectCommand = sqlComand;


                // set get data
                dbConnection.SetAdapter.Fill(dstable);


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                if (dbConnection.canClose) dbConnection.DbConnectionBase.Close();
            }


            return dstable.Tables[0];

        }
              

        #endregion

    }
}
