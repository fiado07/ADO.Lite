using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Lite.Enums;
using System.Data.SqlClient;

namespace ADO.Lite.Tests.Class
{
    public class SqlServerContext : Contracts.IDbConnectionSql
    {

        private string SqlConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=escola;server=WEMZ1BGID1\SQLEXPRESS";

        public bool canClose { get; set; }

        public IEnumerable<string> columnsToFilter { get; set; }

        public IDbConnection DbConnectionBase { get; set; }

        public DbProvider DbProvider { get; set; }

        public IDbDataAdapter SetAdapter { get; set; }

        public SqlServerContext()
        {

            DbConnectionBase = new SqlConnection(SqlConnectionString);

            DbProvider = DbProvider.SqlClient;

            SetAdapter = new SqlDataAdapter();

        }

    }
}

