using ADO.Lite.Contracts;
using ADO.Lite.SqlQuery;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Lite.Example.Class
{
    public class Example
    {
        
        public Example()
        {

            // initialize connecton
            BuildQuery.DbConnection = new SqlServerContext();


        }


        public void CheckAny()
        {

            bool resultado = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 4);


        }



    }
}
