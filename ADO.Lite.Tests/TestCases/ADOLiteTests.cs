using ADO.Lite.Enums;
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

        public ADOLiteTests()
        {
            
            // initialize base connecton
            BuildQuery.DbConnection = new SqlServerContext();

        }

        [Test]
        public void CheckAny()
        {
            //Arrange 
            bool result = false;

                                  
            result = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 5);
            // Act
            result = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 5);


            //Assert
            Assert.IsTrue(result);


        }


        [Test]
        public void InsertBySql()
        {

            // arrange
            string sql = "Insert into aluno (nota,curso,data) values ('30','BBBB',getdate())";
            int totalBefor = 0;
            int totalAfter = 0;


            // act
            totalBefor = BuildQuery.Count(DbProvider.SqlClient, "aluno");

            // Insert query execution
            BuildQuery.ExecuteSql(sql);

            totalAfter = BuildQuery.Count(DbProvider.SqlClient, "aluno");


            // assert 
            Assert.Greater(totalAfter, totalBefor);


        }
        [Test]
        public void InsertByObject()
        {

            // arrange
            string sql = string.Empty;
            int totalBefor = 0;
            int totalAfter = 0;

            Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            // act
            totalBefor = BuildQuery.Count(DbProvider.SqlClient, "aluno");

            // Insert query execution
            BuildQuery.Insert(aluno, new List<string> { nameof(aluno.alunoID), nameof(aluno.data) });

            totalAfter = BuildQuery.Count(DbProvider.SqlClient, "aluno");


            // assert 
            Assert.Greater(totalAfter, totalBefor);


        }

        [Test]
        public void UpdateByObject()
        {

            // arrange

            Aluno alunoCurrent = new Aluno();
            Aluno alunoToUpdate = new Aluno { curso = "20", Nome = "Fiado" };
            Aluno alunoUpdated = new Aluno();


            // act

            alunoCurrent = BuildQuery.GetObject<Aluno>(x => x.alunoID == 5);


            // Insert query execution
            BuildQuery.Update(alunoToUpdate, x => x.alunoID == 5, excludeColumns: new List<string> { nameof(alunoUpdated.data) });


            alunoUpdated = BuildQuery.GetObject<Aluno>(x => x.alunoID == 5);


            // assert 
            Assert.AreNotSame(alunoCurrent.Nome, alunoUpdated.Nome);


        }
        

        [Test]
        public void DeleteByObject()
        {

            // arrange
            string sql = string.Empty;
            int totalBefor = 0;
            int totalAfter = 0;

            Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };


            // act
            totalBefor = BuildQuery.Count(DbProvider.SqlClient, "aluno");

            // Insert query execution
            BuildQuery.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");

            totalAfter = BuildQuery.Count(DbProvider.SqlClient, "aluno");


            // assert 
            Assert.Less(totalAfter, totalBefor);


        }



        [Test]
        public void ExecuteSqlGetTable()
        {
            // arrange
            System.Data.DataTable table = new System.Data.DataTable();
            int total = 0;
            int totalTable = 0;

            // act
            total = BuildQuery.Count<Aluno>(x => x.nota == "10", "aluno");

            table = BuildQuery.GetTable<Aluno>(x => x.nota == "10");

            totalTable = table.Rows.Count;


            // assert

            Assert.AreEqual(total, totalTable);

        }


        [Test]
        public void ExecuteSqlGetObject()
        {
            // arrange
            Aluno aluno = new Aluno();


            // act
            aluno = BuildQuery.GetObject<Aluno>(x => x.alunoID == 4);


            // assert
            Assert.AreEqual(aluno.Nome, "Fiado");

        }

        [Test]
        public void ExecuteSqlGetObjectList()
        {
            // arrange
            IEnumerable<Aluno> aluno;


            // act
            aluno = BuildQuery.GetObjectList<Aluno>(x => x.nota == "10");


            // assert
            Assert.AreEqual(aluno.Count(), 3);

        }





    }
}
