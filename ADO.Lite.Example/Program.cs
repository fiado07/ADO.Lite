using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Lite;
using ADO.Lite.Contracts;
using System.Data.SqlClient;
using System.Data;
using ADO.Lite.SqlQuery;
using ADO.Lite.Example.Class;
using ADO.Lite.Enums;

namespace ADO.Lite.Example
{
    class Program
    {
        static void Main(string[] args)
        {


            IDbConnectionSql sqlConnnection = new SqlServerContext();
            BuildQuery.DbConnection = sqlConnnection;



            // arrange
            string sql = string.Empty;
            int totalBefor = 0;
            int totalAfter = 0;

            Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            // act
            totalBefor = BuildQuery.Count("aluno");

            // Insert query execution
            BuildQuery.Insert(aluno, new List<string> { nameof(aluno.alunoID), nameof(aluno.data) });

            totalAfter = BuildQuery.Count("aluno");


            //Aluno aluno = new Aluno { alunoID = 4, nota = "10" };

            bool resultado = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 4);


            Console.WriteLine(resultado);
            Console.Read();


        }


        public static void InsertByObject()
        {

            // arrange
            string sql = string.Empty;
            int totalBefor = 0;
            int totalAfter = 0;

            Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            // act
            totalBefor = BuildQuery.Count( "aluno");

            // Insert query execution
            BuildQuery.Insert(aluno, new List<string> { nameof(aluno.data) });

            totalAfter = BuildQuery.Count( "aluno");


            // assert 
            //   Assert.Greater(totalAfter, totalBefor);


        }

    }
}
