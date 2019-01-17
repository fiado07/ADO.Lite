using ADO.Lite.Contracts;
using ADO.Lite.SqlQuery;
using ADO.Lite.Tests.Class;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ADO.Lite.Tests.TestCases
{
    [TestFixture]
    public class ADOLiteTests
    {

        BuildQuery buildquery;

        public ADOLiteTests()
        {

            IDbConnectionSql Db2Connnection = new Tests.Class.SqlServerContext();
            // BuildQuery.DbConnection = Db2Connnection;

            buildquery = BuildQuery.ContextBuilder(Db2Connnection);


        }

        [Test]
        public void CheckAny()
        {
            //Arrange 
            bool result = false;


            //result = buildquery.any<Aluno>((x) => x.nota == "10" && x.alunoID == 5);

            // Act
            result = buildquery.any<Aluno>((x) => x.nota == "10" && x.alunoID == 19);


            //Assert
            Assert.IsTrue(result);


        }


        [Test]
        public void InsertBySql()
        {

            // arrange
            string sql = "Insert into aluno (nota,curso,data) values ('30','BBBB',getdate())";
            int count = 0;
            int countAfter = 0;
            Parameters.SqlAndParameters sqlParameters = new Parameters.SqlAndParameters();


            sqlParameters.Sql = sql;

            count = (int)buildquery.ExecuteSqlGetSingle(new Parameters.SqlAndParameters() { Sql = "Select count(*) total from aluno" });


            // Insert query execution
            buildquery.ExecuteSql(sqlParameters);


            countAfter = (int)buildquery.ExecuteSqlGetSingle(new Parameters.SqlAndParameters() { Sql = "Select count(*) total from aluno" });

            // assert 
            Assert.Greater(countAfter, count);


        }
        [Test]
        public void InsertByObject()
        {

            // arrange
            string sql = string.Empty;


            Aluno aluno = new Aluno { curso = "20", Nome = "Fia", nota = "11" };

            // Insert query execution
            buildquery.Add(aluno, new List<string> { nameof(aluno.alunoID), nameof(aluno.data) });


            // assert 
            Assert.IsTrue(true);

        }

        [Test]
        public void UpdateByObject()
        {

            // arrange

            Aluno alunoCurrent = new Aluno();
            Aluno alunoToUpdate = new Aluno { curso = "20", Nome = "Fiado" };
            Aluno alunoUpdated = new Aluno();
            

            // Insert query execution
            buildquery.Update(alunoToUpdate, x => x.alunoID == 19, new List<string> { nameof(alunoUpdated.data), nameof(alunoUpdated.nota) });


            alunoUpdated = buildquery.Get<Aluno>(x => x.alunoID == 19);


            // assert 
            Assert.AreNotSame(alunoCurrent.Nome, alunoUpdated.Nome);


        }


        [Test]
        public void DeleteByObject()
        {

            // arrange
            string sql = string.Empty;
     
            Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            // act
            // totalBefor = BuildQuery.Count("aluno");

            // query execution
            buildquery.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");

            // totalAfter = BuildQuery.Count("aluno");


            // assert 
            // Assert.Less(totalAfter, totalBefor);

            Assert.IsTrue(true);
        }



        [Test]
        public void ExecuteSqlGetTable()
        {
            // arrange
            System.Data.DataTable table = null;
            //int total = 0;
            //int totalTable = 0;

            // act
            // total = BuildQuery.Count<Aluno>(x => x.nota == "10", "aluno");

            table = buildquery.ExecuteSqlGetTabela(new Parameters.SqlAndParameters() { Sql = "Select * from aluno" });

            // totalTable = table.Rows.Count;
            

            // assert

            Assert.IsNotNull(table);

        }


        [Test]
        public void ExecuteSqlGetObject()
        {
            // arrange
            Aluno aluno = new Aluno();


            // act
            aluno = buildquery.Get<Aluno>(x => x.alunoID == 19);


            // assert
            Assert.AreEqual(aluno.Nome, "Fiado");

        }

        [Test]
        public void ExecuteSqlGetObjectList()
        {
            // arrange
            IEnumerable<Aluno> aluno;


            // act
            aluno = buildquery.GetList<Aluno>(x => x.nota == "40");


            // assert
            Assert.AreEqual(aluno.Count(), 2);

        }

    }
}
