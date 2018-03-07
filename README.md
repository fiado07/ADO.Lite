<h1>ADO.Lite - Is a lite ORM for .Net</h1>
<h2></h2>
<h1></h1>
<h1></h1>
<p>The <strong>ADO.Lite</strong> is a powerfull ORM lite for .Net apps that provides an intuitive and flexible way to work with relational databases, with intelicense privided on arguments as lambda expression delegates.</p>
<p></br>
</br></p>
<h2>Database supported</h2>
<p>The ADO.Lite supports</p>
<ul>
<li>Sql Server</li>
<li>MySql</li>
<li>DB2</li>
</ul>
<h2>Context</h2>
<p>Creating context to connect to database. First of all, must be defined the context class that will be responsable to connect to database.
The Context class must implement this abstraction:</p>
<p><code>Contracts.IDbConnectionSql</code> </p>
<pre>
 <code>
<span style="color:blue;">public class</span> <span style="color:green;">SqlServerContext</span> : Contracts.<span style="color:#cccc00;">IDbConnectionSql</span>
    {

        <span style="color:blue;">private string</span> SqlConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=;server=";

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
<p></br></p>
<h2>Connect to database</h2>
<h1></h1>
<p>We only need to initialize the context class to associate it to database provider on Domain Class.</p>
<pre>
 <code>
  <span style="color:blue;">public</span> ADOLiteTests()
   {   
    // initialize base connecton
    <span style="color:green;">BuildQuery</span>.DbConnection =<span style="color:blue;">new</span><span style="color:green">SqlServerContext</span>();
   }
 </code>
</pre>
<p></br></p>
<h2>Check data on database</h2>
<h1></h1>
<p>This is strongly typed function that takes lambda expression predicate as argument to build select query. </p>
<pre>
 <code >
 result = <span style="color:green;">BuildQuery</span>.CheckAny<<span style="color:green;">Aluno</span>>((x) => x.nota ==  <span style="color:brown;">"10"</span> && x.alunoID == 5);
 </code>
</pre>
<p></br></p>
<h2>Execute query with no return</h2>
<h1></h1>
<p>This function only execute a plane query to database. </p>
<pre>
 <code >
  <span style="color:green;">BuildQuery</span>.ExecuteSql(sql);
 </code>
</pre>
<p></br></p>
<h2>Insert data to database</h2>
<h1></h1>
<p>This function builds <strong>INSERT</strong> query by object and can exclude some properties if needed.</p>
<pre>
 <code >
  <span style="color:green;"> Aluno</span> aluno = new  <span style="color:green;">Aluno</span> { curso = "20", Nome = "Fia" };  
  // Insert query execution
  <span style="color:green;">BuildQuery</span>.Insert(aluno, new <span style="color:green;">List</span><<span style="color:blue;">string</span>> { <span style="color:blue;">nameof</span>>(aluno.alunoID), nameof(aluno.data) });
 </code>
</pre>
<p></br></p>
<h2>UPDATE data on database</h2>
<h1></h1>
<p>This function builds <strong>UPDATE</strong> query by object and can exclude some properties if needed.</p>
<pre>
 <code >
 <span style="color:green;">Aluno</span> alunoToUpdate =<span style="color:blue;">new</span>  <span style="color:green;">Aluno</span> { curso = "20", Nome = "Fiado" };

// Insert query execution
<span style="color:green;">BuildQuery</span>.Update(alunoToUpdate, x => x.alunoID == 5, excludeColumns: new List<string> { nameof(alunoUpdated.data) });
 </code>
</pre>
<p></br></p>
<h2>DELETE data on database</h2>
<h1></h1>
<p>This is strongly typed function that builds <strong>DELETE</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// delete
   <span style="color:green;">BuildQuery</span>.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");
 </code>
</pre>
<p></br></p>
<h2>GET DataTable Result from database</h2>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// get
    DataTable = <span style="color:green;">BuildQuery</span>.GetTable<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
<p></br></p>
<h2>GET Single Object from database</h2>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// select
  aluno = <span style="color:green;">BuildQuery</span>.GetObject<<span style="color:blue;">Aluno</span>>(x => x.alunoID == 4);
 </code>
</pre>
<p></br></p>
<h2>GET Object List from database</h2>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// select
   aluno = <span style="color:green;">BuildQuery</span>.GetObjectList<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
<p></br></p>
<h2>Notes</h2>
<h1></h1>
<p>This library olds transactions too, just need to set it on initialization in this property:</p>
<pre>
 <code >
 <span style="color:green;">BuildQuery</span>.DbConnection.DbConnectionBase.BeginTransaction
 </code>
</pre>
<p></br></p>
<h2>Requirements</h2>
<h1></h1>
<p><strong>ADO.Lite</strong> requires .NET framework 4.6 +</p>
