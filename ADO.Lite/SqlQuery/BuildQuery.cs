
using ADO.Lite.Contracts;
using ADO.Lite.Parameters;
using ADO.Lite.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;


namespace ADO.Lite.SqlQuery
{
    /// <summary>
    /// Main Entrance class to build queries
    /// </summary>
    public class BuildQuery : IDbExecute
    {
        //IDbConnectionSql DbConnection { set; get; }

        private static DbExecuteQuery DbExecuteQuery { set; get; }


        private BuildQuery()
        {

        }


        public static BuildQuery ContextBuilder(IDbConnectionSql dbConnection)
        {

            DbExecuteQuery = new DbExecuteQuery(dbConnection);
            
            return new SqlQuery.BuildQuery(); 

        }


        public void Add<T>(T dataObject, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            DbExecuteQuery.Add(dataObject, excludeProperties, isStoredProcedure);
        }

        public bool any(SqlAndParameters sqlParameter)
        {
            return DbExecuteQuery.any(sqlParameter);
        }

        public bool any<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return DbExecuteQuery.any(predicate);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            DbExecuteQuery.Delete(predicate);
        }

        public void ExecuteSql(SqlAndParameters sqlParameter)
        {
            DbExecuteQuery.ExecuteSql(sqlParameter);
        }

        public T ExecuteSqlGetObject<T>(SqlAndParameters sqlParameter) where T : new()
        {
            return DbExecuteQuery.ExecuteSqlGetObject<T>(sqlParameter);
        }

        public IEnumerable<T> ExecuteSqlGetObjectList<T>(SqlAndParameters sqlParameter) where T : new()
        {
            return DbExecuteQuery.ExecuteSqlGetObjectList<T>(sqlParameter);
        }

        public object ExecuteSqlGetSingle(SqlAndParameters sqlParameter)
        {
            return DbExecuteQuery.ExecuteSqlGetSingle(sqlParameter);
        }

        public DataTable ExecuteSqlGetTabela(SqlAndParameters sqlParameter)
        {
            return DbExecuteQuery.ExecuteSqlGetTabela(sqlParameter);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return DbExecuteQuery.Get(predicate);
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return DbExecuteQuery.GetList(predicate);
        }

        public void Update<T>(T dataObject, string predicate, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            DbExecuteQuery.Update(dataObject, predicate, excludeProperties, isStoredProcedure);
        }

        public void Update<T>(T dataObject, Expression<Func<T, bool>> predicate, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            DbExecuteQuery.Update(dataObject, predicate, excludeProperties, isStoredProcedure);
        }
    }
}
