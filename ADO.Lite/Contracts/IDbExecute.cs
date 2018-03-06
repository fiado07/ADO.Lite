
using System.Collections.Generic;

namespace ADO.Lite.Contracts
{
    /// <summary>
    /// Structure api to interact to db
    /// </summary>
    public interface IDbExecute
    {

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        void ExecuteSql(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection);

        /// <summary>
        /// Executes the SQL get object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        T ExecuteSqlGetObject<T>(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection) where T : new();


        /// <summary>
        /// Check if is there any data
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool any(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection);


        /// <summary>
        /// Executes the SQL get object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteSqlGetObjectList<T>(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection) where T : new();


        /// <summary>
        /// Executes the SQL get tabela.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        System.Data.DataTable ExecuteSqlGetTabela(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection);


        int ExecuteSqlGetSingle(Parameters.SqlParameter sqlParameter, IDbConnectionSql dbConnection);


    }
}
