
using System;
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
        void ExecuteSql(Parameters.SqlAndParameters sqlParameter);

        /// <summary>
        /// Executes the SQL get object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        T ExecuteSqlGetObject<T>(Parameters.SqlAndParameters sqlParameter) where T : new();


        /// <summary>
        /// Check if is there any data
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool any(Parameters.SqlAndParameters sqlParameter);


        /// <summary>
        /// Executes the SQL get object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteSqlGetObjectList<T>(Parameters.SqlAndParameters sqlParameter) where T : new();


        /// <summary>
        /// Executes the SQL get tabela.
        /// </summary>
        /// <param name="sqlParameter">The SQL parameter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        System.Data.DataTable ExecuteSqlGetTabela(Parameters.SqlAndParameters sqlParameter);


        object ExecuteSqlGetSingle(Parameters.SqlAndParameters sqlParameter);

        
        void Add<T>(T dataObject, List<string> excludeProperties = null, bool isStoredProcedure = false);

        void Update<T>(T dataObject, string predicate, List<string> excludeProperties = null, bool isStoredProcedure = false);

        void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        void Update<T>(T dataObject, System.Linq.Expressions.Expression<Func<T, bool>> predicate, List<string> excludeProperties = null, bool isStoredProcedure = false);
        
        T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        IEnumerable<T> GetList<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        bool any<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();



    }
}
