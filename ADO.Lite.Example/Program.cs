using System;
using System.Collections.Generic;
using ADO.Lite.Contracts;
using ADO.Lite.SqlQuery;
using ADO.Lite.Example.Class;


namespace ADO.Lite.Example
{
    class Program
    {
        static void Main(string[] args)
        {


            //command.CommandText = "Select TLELEM, TLDESC from SIMCTLC.TBLLIN where TLCODE=640 "


            IDbConnectionSql Db2Connnection = new Tests.Class.DB2Context();
            // BuildQuery.DbConnection = Db2Connnection;
            
            BuildQuery buildquery = BuildQuery.ContextBuilder(Db2Connnection) ;
                    
            object result;

            try
            {

                result = buildquery.Get<Matricula>(x => x.DMACCT == "250101620");
                
            }
            catch (Exception)
            {

                throw;
            }



            //IDbConnectionSql sqlConnnection = new SqlServerContext();
            //BuildQuery.DbConnection = sqlConnnection;



            //// arrange
            //string sql = string.Empty;
            //int totalBefor = 0;
            //int totalAfter = 0;

            //Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            //// act
            //totalBefor = BuildQuery.Count("aluno");

            //// Insert query execution
            //BuildQuery.Insert(aluno, new List<string> { nameof(aluno.alunoID), nameof(aluno.data) });

            //totalAfter = BuildQuery.Count("aluno");


            ////Aluno aluno = new Aluno { alunoID = 4, nota = "10" };

            //bool resultado = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 4);


            //Console.WriteLine(resultado);
            Console.Read();


        }


        public class Matricula
        {

            public string DMACCT { get; set; }
            public string DMBK { get; set; }

            public string DMTYP { get; set; }

            public Matricula()
            {

            }



        }


    }
}
