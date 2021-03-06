﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Lite.Enums;
using System.Data.SqlClient;
using ADO.Lite.Contracts;

namespace ADO.Lite.Tests.Class
{
    public class SqlServerContext : Contracts.IDbConnectionSql
    {

        private string SqlConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=escola;Data Source=.\sqlexpress";


        public bool canClose { get; set; }

        public IEnumerable<string> columnsToFilter { get; set; }

        public IDbConnection DbConnectionBase { get; set; }

        IDbTransaction IDbConnectionSql.DbTransaction { get; set; }

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

