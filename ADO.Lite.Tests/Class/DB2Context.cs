using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Lite.Enums;
using System.Data.Odbc;
using ADO.Lite.Contracts;

namespace ADO.Lite.Tests.Class
{
    public class DB2Context : Contracts.IDbConnectionSql
    {


        public bool canClose { get; set; }

        public IEnumerable<string> columnsToFilter { get; set; }

        public IDbConnection DbConnectionBase { get; set; }


        public DbProvider DbProvider { get; set; }

        public IDbDataAdapter SetAdapter { get; set; }

        IDbTransaction IDbConnectionSql.DbTransaction { get; set; }

        public DB2Context()
        {

            DbConnectionBase = new OdbcConnection("Provider=MSDASQL;DSN=; UID=; PWD=");

            DbProvider = DbProvider.DB2Client;

            SetAdapter = new OdbcDataAdapter();

        }
    }
}
