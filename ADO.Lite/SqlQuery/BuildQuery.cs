
using ADO.Lite.Contracts;
using ADO.Lite.Enums;
using ADO.Lite.Expressions;
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
    public static class BuildQuery
    {
        public static IDbConnectionSql DbConnection { set; get; }

        private static DbExecuteQuery dbExecuteQuery { set; get; }


        public static void InitializeBuildQuery()
        {

            dbExecuteQuery = new DbExecuteQuery();

            if (DbConnection == null)
                throw new Exception("DbConnection is not Initialized.");

            if (DbConnection.DbConnectionBase.State == ConnectionState.Closed)
                DbConnection.DbConnectionBase.Open();


        }

        public static bool CheckAny<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {

            bool _result = false;
            SqlParameter sqlAndParameter = new SqlParameter();
            //Expression<Func<T, Boolean>> predicate;


            // check initializers
            InitializeBuildQuery();


            // get parameters
            sqlAndParameter = SqlExpressionBuilder.sqlBuildSelect(predicate, nomeTabela);


            // shearch for result
            _result = dbExecuteQuery.any(sqlAndParameter, DbConnection);

            return _result;

        }


        public static int Count<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {

            int _result = 0;
            SqlParameter sqlAndParameter = new SqlParameter();


            // check initializers
            InitializeBuildQuery();


            // get parameters
            sqlAndParameter = SqlExpressionBuilder.sqlBuildSelectCount(predicate, nomeTabela);


            // shearch for result
            _result = dbExecuteQuery.ExecuteSqlGetSingle(sqlAndParameter, DbConnection);

            return _result;

        }


        public static int Count(this DbProvider dbProvider, string nomeTabela = "")
        {

            int _result = 0;
            SqlParameter sqlAndParameter = new SqlParameter();


            // check initializers
            InitializeBuildQuery();


            // get parameters
            sqlAndParameter = SqlExpressionBuilder.sqlBuildSelectCountAll(nomeTabela);


            // shearch for result
            _result = dbExecuteQuery.ExecuteSqlGetSingle(sqlAndParameter, DbConnection);

            return _result;

        }


        public static void ExecuteSql(this string sql)
        {

            try
            {

                // initializers
                InitializeBuildQuery();

                // execute
                dbExecuteQuery.ExecuteSql(new SqlParameter { Sql = sql }, DbConnection);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static DataTable GetTable<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {

            DataTable tabela = new DataTable();
            SqlParameter sqlParameter = null;

            try
            {

                // initializers
                InitializeBuildQuery();


                // get params
                sqlParameter = SqlExpressionBuilder.sqlBuildSelect(predicate, nomeTabela);


                // execute
                tabela = dbExecuteQuery.ExecuteSqlGetTabela(sqlParameter, DbConnection);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return tabela;

        }

        public static T GetObject<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "") where T : new()
        {

            DataTable _tabela = new DataTable();
            T _entity = new T();
            SqlParameter sqlParameter = null;

            try
            {

                // initializers
                InitializeBuildQuery();


                // get params
                sqlParameter = SqlExpressionBuilder.sqlBuildSelect(predicate, nomeTabela);


                // execute
                _entity = dbExecuteQuery.ExecuteSqlGetObject<T>(sqlParameter, DbConnection);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return _entity;

        }

        public static string ExecuteSqlGetSingle(this SqlParameter sqlParameter)
        {

            string result = string.Empty;
            string filterField = sqlParameter.Sql.ToLower().Split(new string[] { "select" }, StringSplitOptions.None)[0];

            try
            {


                // initializers
                InitializeBuildQuery();


                // execute
                result = dbExecuteQuery.ExecuteSqlGetTabela(sqlParameter, DbConnection).Rows[0][filterField].ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;

        }

        public static IEnumerable<T> GetObjectList<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "") where T : new()
        {

            IEnumerable<T> listObject = new List<T>();
            SqlParameter sqlParameter = null;

            try
            {


                // initializers
                InitializeBuildQuery();


                // get params
                sqlParameter = SqlExpressionBuilder.sqlBuildSelect(predicate, nomeTabela);


                // execute
                listObject = dbExecuteQuery.ExecuteSqlGetObjectList<T>(sqlParameter, DbConnection);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return listObject;

        }


        public static void Insert<T>(T newObject, List<string> excludeColumns = null, string nomeTabela = "")
        {

            SqlParameter sqlParameter = new SqlParameter();

            try
            {


                // get parameters
                sqlParameter = SqlExpressionBuilder.sqlBuildInsert(newObject, nomeTabela, excludeColumns);


                // execute
                dbExecuteQuery.ExecuteSql(sqlParameter, DbConnection);


            }
            catch (Exception)
            {

                throw;
            }


        }


        public static void Update<T>(T newObject, Expression<Func<T, Boolean>> predicate, string nomeTabela = "", List<string> excludeColumns = null) where T : new()
        {

            SqlParameter sqlParameter = new SqlParameter();


            try
            {

                // get parameters
                sqlParameter = SqlExpressionBuilder.sqlBuildUpdate(newObject, predicate, nomeTabela, excludeColumns);


                if (string.IsNullOrEmpty(sqlParameter.Sql)) throw new Exception("Nenhum dado foi modifiado.");

                // execute
                dbExecuteQuery.ExecuteSql(sqlParameter, DbConnection);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        

        public static void Delete<T>(this Expression<Func<T, Boolean>> predicate, string nomeTabela = "")
        {
            SqlParameter sqlParameter = new SqlParameter();

            try
            {

                // get parameters
                sqlParameter = SqlExpressionBuilder.sqlBuidDelete(predicate, nomeTabela);


                // execute
                dbExecuteQuery.ExecuteSql(sqlParameter, DbConnection);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        
    }
}
