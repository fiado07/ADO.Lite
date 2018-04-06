

# ADO.Lite - Is a lite ORM for .Net #

The **ADO.Lite** is a powerfull ORM lite for .Net apps that provides an intuitive and flexible way to work with relational databases, with intelicense privided on arguments as lambda expression delegates.



### Database supported ###

The ADO.Lite supports

- Sql Server
- MySql
- DB2

### Context ###

Creating context to connect to database. First of all, must be defined the context class that will be responsable to connect to database.
The Context class must implement this abstraction:

```cs
Contracts.IDbConnectionSql 
```


```csharp
  public class SqlServerContext : Contracts.IDbConnectionSql
    {

        private string SqlConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=;server=";

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
```



### Connect to database ###
 We only need to initialize the context class to associate it to database provider on Domain Class.

```c#
public ADOLiteTests()
{
// initialize base connecton
 BuildQuery.DbConnection = new SqlServerContext();
}
```



### Check data on database ###

This is strongly typed function that takes lambda expression predicate as argument to build select query. 

```c#
   result = BuildQuery.CheckAny<Aluno>((x) => x.nota == "10" && x.alunoID == 5);
```


### Execute query with no return ###

This function only execute a plane query to database. 

```csharp
BuildQuery.ExecuteSql(sql);
```



### Insert data to database ###

This function builds **INSERT** query by object and can exclude some properties if needed.

```csharp
Aluno aluno = new Aluno { curso = "20", Nome = "Fia" };
 
// Insert query execution
 BuildQuery.Insert(aluno, new List<string> { nameof(aluno.alunoID), nameof(aluno.data) });
```



### UPDATE data on database ###

This function builds **UPDATE** query by object and can exclude some properties if needed.

```csharp
Aluno alunoToUpdate = new Aluno { curso = "20", Nome = "Fiado" };

// Insert query execution
BuildQuery.Update(alunoToUpdate, x => x.alunoID == 5, excludeColumns: new List<string> { nameof(alunoUpdated.data) });

```



### DELETE data on database ###
This is strongly typed function that builds **DELETE** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

```csharp
// query execution
BuildQuery.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");
```



### GET DataTable Result from database ###

This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

```csharp
table = BuildQuery.GetTable<Aluno>(x => x.nota == "10");
```



### GET Single Object from database ###

This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

```csharp
aluno = BuildQuery.GetObject<Aluno>(x => x.alunoID == 4);
```



### GET Object List from database ###

This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

```csharp
aluno = BuildQuery.GetObjectList<Aluno>(x => x.nota == "10");
```



### Notes ###

This library holds transactions too, just need to set it on initialization in this property:

```csharp
BuildQuery.DbConnection.DbConnectionBase.BeginTransaction
```




### Requirements ###
**ADO.Lite** requires .NET framework 4 +