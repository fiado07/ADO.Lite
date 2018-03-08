

# ADO.Lite - Is a lite ORM for .Net #
</br>


The **ADO.Lite** is a powerfull ORM lite for .Net apps that provides an intuitive and flexible way to work with relational databases, with intelicense privided on arguments as lambda expression delegates.
 
</br>
</br>

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
<pre>
<code>

public class SqlServerContext : Contracts.<span style="color:red">IDbConnectionSql</span>
    {

        <span style="color:blue">private string</span> SqlConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=;server=";

        <span style="color:blue;">public bool</span> canClose { <span style="color:blue;">get</span>; <span style="color:blue;">set</span>; }

        <span style="color:blue;">public</span> <span style="color:#cccc00;">IEnumerable<string></span> columnsToFilter { <span style="color:blue;">get</span>; <span style="color:blue;">set</span>; }

         <span style="color:blue;">public</span> <span style="color:#cccc00;">IDbConnection<string></span>  DbConnectionBase { <span style="color:blue;">get</span>; <span style="color:blue;">set</span>; }

         <span style="color:blue;">public</span> <span style="color:#cccc00;">DbProvider</span>  DbProvider { <span style="color:blue;">get</span>; <span style="color:blue;">set</span>; }

         <span style="color:blue;">public</span> <span style="color:#cccc00;">IDbDataAdapter<string></span>  SetAdapter { <span style="color:blue;">get</span>; <span style="color:blue;">set</span>; }

        public SqlServerContext()
        {

            DbConnectionBase = <span style="color:blue;">new</span>  <span style="color:green;">SqlConnection</span> (SqlConnectionString);

            DbProvider = <span style="color:#cccc00;">DbProvider</span> .SqlClient;

            SetAdapter =  <span style="color:blue;">new</span>  <span style="color:green;">SqlDataAdapter</span>();
        }

    }


</code>

</pre>



</br>
### Connect to database ###
# 

 We only need to initialize the context class to associate it to database provider on Domain Class.

<pre>
 <code>
  <span style="color:blue;">public</span> ADOLiteTests()
   {   
    // initialize base connecton
    <span style="color:green;">BuildQuery</span>.DbConnection =<span style="color:blue;">new</span><span style="color:green">SqlServerContext</span>();
   }
 </code>
</pre>
</br>

### Check data on database ###
# 
This is strongly typed function that takes lambda expression predicate as argument to build select query. 

```
 result = <span style="color:green;">BuildQuery</span>.CheckAny<<span style="color:green;">Aluno</span>>((x) => x.nota ==  <span style="color:brown;">"10"</span> && x.alunoID == 5);
```
</br>
### Execute query with no return ###
# 
This function only execute a plane query to database. 

<pre>
 <code  >
  <span style="color:green;">BuildQuery</span>.ExecuteSql(sql);
 </code>
</pre>
</br>

### Insert data to database ###
# 
This function builds **INSERT** query by object and can exclude some properties if needed.

<pre>
 <code >
  <span style="color:green;"> Aluno</span> aluno = new  <span style="color:green;">Aluno</span> { curso = "20", Nome = "Fia" };  
  // Insert query execution
  <span style="color:green;">BuildQuery</span>.Insert(aluno, new <span style="color:green;">List</span><<span style="color:blue;">string</span>> { <span style="color:blue;">nameof</span>>(aluno.alunoID), nameof(aluno.data) });
 </code>
</pre>
</br>

### UPDATE data on database ###
# 
This function builds **UPDATE** query by object and can exclude some properties if needed.

<pre>
 <code >
 <span style="color:green;">Aluno</span> alunoToUpdate =<span style="color:blue;">new</span>  <span style="color:green;">Aluno</span> { curso = "20", Nome = "Fiado" };

// Insert query execution
<span style="color:green;">BuildQuery</span>.Update(alunoToUpdate, x => x.alunoID == 5, excludeColumns: new List<string> { nameof(alunoUpdated.data) });
 </code>
</pre>
</br>

### DELETE data on database ###
# 
This is strongly typed function that builds **DELETE** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

<pre>
 <code >
// delete
   <span style="color:green;">BuildQuery</span>.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");
 </code>
</pre>
</br>

### GET DataTable Result from database ###
# 
This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

<pre>
 <code >
// get
    DataTable = <span style="color:green;">BuildQuery</span>.GetTable<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
</br>

### GET Single Object from database ###
# 
This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

<pre>
 <code >
// select
  aluno = <span style="color:green;">BuildQuery</span>.GetObject<<span style="color:blue;">Aluno</span>>(x => x.alunoID == 4);
 </code>
</pre>
</br>

### GET Object List from database ###
# 
This is strongly typed function that builds **SELECT** query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.

<pre>
 <code >
// select
   aluno = <span style="color:green;">BuildQuery</span>.GetObjectList<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
</br>

### Notes ###
# 
This library olds transactions too, just need to set it on initialization in this property:

<pre>
 <code >
 <span style="color:green;">BuildQuery</span>.DbConnection.DbConnectionBase.BeginTransaction
 </code>
</pre>
</br>


### Requirements ###
# 
**ADO.Lite** requires .NET framework 4.6 +