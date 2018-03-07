<!DOCTYPE html>
<html>
<head>
<title>README</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<style type="text/css">
/* GitHub stylesheet for MarkdownPad (http://markdownpad.com) */
/* Author: Nicolas Hery - http://nicolashery.com */
/* Version: b13fe65ca28d2e568c6ed5d7f06581183df8f2ff */
/* Source: https://github.com/nicolahery/markdownpad-github */

/* RESET
=============================================================================*/

html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, b, u, i, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary, time, mark, audio, video {
  margin: 0;
  padding: 0;
  border: 0;
}

/* BODY
=============================================================================*/

body {
  font-family: Helvetica, arial, freesans, clean, sans-serif;
  font-size: 14px;
  line-height: 1.6;
  color: #333;
  background-color: #fff;
  padding: 20px;
  max-width: 960px;
  margin: 0 auto;
}

body>*:first-child {
  margin-top: 0 !important;
}

body>*:last-child {
  margin-bottom: 0 !important;
}

/* BLOCKS
=============================================================================*/

p, blockquote, ul, ol, dl, table, pre {
  margin: 15px 0;
}

/* HEADERS
=============================================================================*/

h1, h2, h3, h4, h5, h6 {
  margin: 20px 0 10px;
  padding: 0;
  font-weight: bold;
  -webkit-font-smoothing: antialiased;
}

h1 tt, h1 code, h2 tt, h2 code, h3 tt, h3 code, h4 tt, h4 code, h5 tt, h5 code, h6 tt, h6 code {
  font-size: inherit;
}

h1 {
  font-size: 28px;
  color: #000;
}

h2 {
  font-size: 24px;
  border-bottom: 1px solid #ccc;
  color: #000;
}

h3 {
  font-size: 18px;
}

h4 {
  font-size: 16px;
}

h5 {
  font-size: 14px;
}

h6 {
  color: #777;
  font-size: 14px;
}

body>h2:first-child, body>h1:first-child, body>h1:first-child+h2, body>h3:first-child, body>h4:first-child, body>h5:first-child, body>h6:first-child {
  margin-top: 0;
  padding-top: 0;
}

a:first-child h1, a:first-child h2, a:first-child h3, a:first-child h4, a:first-child h5, a:first-child h6 {
  margin-top: 0;
  padding-top: 0;
}

h1+p, h2+p, h3+p, h4+p, h5+p, h6+p {
  margin-top: 10px;
}

/* LINKS
=============================================================================*/

a {
  color: #4183C4;
  text-decoration: none;
}

a:hover {
  text-decoration: underline;
}

/* LISTS
=============================================================================*/

ul, ol {
  padding-left: 30px;
}

ul li > :first-child, 
ol li > :first-child, 
ul li ul:first-of-type, 
ol li ol:first-of-type, 
ul li ol:first-of-type, 
ol li ul:first-of-type {
  margin-top: 0px;
}

ul ul, ul ol, ol ol, ol ul {
  margin-bottom: 0;
}

dl {
  padding: 0;
}

dl dt {
  font-size: 14px;
  font-weight: bold;
  font-style: italic;
  padding: 0;
  margin: 15px 0 5px;
}

dl dt:first-child {
  padding: 0;
}

dl dt>:first-child {
  margin-top: 0px;
}

dl dt>:last-child {
  margin-bottom: 0px;
}

dl dd {
  margin: 0 0 15px;
  padding: 0 15px;
}

dl dd>:first-child {
  margin-top: 0px;
}

dl dd>:last-child {
  margin-bottom: 0px;
}

/* CODE
=============================================================================*/

pre, code, tt {
  font-size: 12px;
  font-family: Consolas, "Liberation Mono", Courier, monospace;
}

code, tt {
  margin: 0 0px;
  padding: 0px 0px;
  white-space: nowrap;
  border: 1px solid #eaeaea;
  background-color: #f8f8f8;
  border-radius: 3px;
}

pre>code {
  margin: 0;
  padding: 0;
  white-space: pre;
  border: none;
  background: transparent;
}

pre {
  background-color: #f8f8f8;
  border: 1px solid #ccc;
  font-size: 13px;
  line-height: 19px;
  overflow: auto;
  padding: 6px 10px;
  border-radius: 3px;
}

pre code, pre tt {
  background-color: transparent;
  border: none;
}

kbd {
    -moz-border-bottom-colors: none;
    -moz-border-left-colors: none;
    -moz-border-right-colors: none;
    -moz-border-top-colors: none;
    background-color: #DDDDDD;
    background-image: linear-gradient(#F1F1F1, #DDDDDD);
    background-repeat: repeat-x;
    border-color: #DDDDDD #CCCCCC #CCCCCC #DDDDDD;
    border-image: none;
    border-radius: 2px 2px 2px 2px;
    border-style: solid;
    border-width: 1px;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    line-height: 10px;
    padding: 1px 4px;
}

/* QUOTES
=============================================================================*/

blockquote {
  border-left: 4px solid #DDD;
  padding: 0 15px;
  color: #777;
}

blockquote>:first-child {
  margin-top: 0px;
}

blockquote>:last-child {
  margin-bottom: 0px;
}

/* HORIZONTAL RULES
=============================================================================*/

hr {
  clear: both;
  margin: 15px 0;
  height: 0px;
  overflow: hidden;
  border: none;
  background: transparent;
  border-bottom: 4px solid #ddd;
  padding: 0;
}

/* TABLES
=============================================================================*/

table th {
  font-weight: bold;
}

table th, table td {
  border: 1px solid #ccc;
  padding: 6px 13px;
}

table tr {
  border-top: 1px solid #ccc;
  background-color: #fff;
}

table tr:nth-child(2n) {
  background-color: #f8f8f8;
}

/* IMAGES
=============================================================================*/

img {
  max-width: 100%
}
</style>
<base href='file:\\\C:\Users\x120801\Desktop\Personal\Git Documentation\'/>
</head>
<body>
<h1>ADO.Lite - Is a lite ORM for .Net</h1>
<h2></h2>
<h1></h1>
<h1></h1>
<p>The <strong>ADO.Lite</strong> is a powerfull ORM lite for .Net apps that provides an intuitive and flexible way to work with relational databases, with intelicense privided on arguments as lambda expression delegates.</p>
<p></br>
</br></p>
<h3>Database supported</h3>
<p>The ADO.Lite supports</p>
<ul>
<li>Sql Server</li>
<li>MySql</li>
<li>DB2</li>
</ul>
<h3>Context</h3>
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
<h3>Connect to database</h3>
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
<h3>Check data on database</h3>
<h1></h1>
<p>This is strongly typed function that takes lambda expression predicate as argument to build select query. </p>
<pre>
 <code >
 result = <span style="color:green;">BuildQuery</span>.CheckAny<<span style="color:green;">Aluno</span>>((x) => x.nota ==  <span style="color:brown;">"10"</span> && x.alunoID == 5);
 </code>
</pre>
<p></br></p>
<h3>Execute query with no return</h3>
<h1></h1>
<p>This function only execute a plane query to database. </p>
<pre>
 <code >
  <span style="color:green;">BuildQuery</span>.ExecuteSql(sql);
 </code>
</pre>
<p></br></p>
<h3>Insert data to database</h3>
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
<h3>UPDATE data on database</h3>
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
<h3>DELETE data on database</h3>
<h1></h1>
<p>This is strongly typed function that builds <strong>DELETE</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// delete
   <span style="color:green;">BuildQuery</span>.Delete<Aluno>(x => x.curso == "20" && x.Nome == "Fia");
 </code>
</pre>
<p></br></p>
<h3>GET DataTable Result from database</h3>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// get
    DataTable = <span style="color:green;">BuildQuery</span>.GetTable<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
<p></br></p>
<h3>GET Single Object from database</h3>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// select
  aluno = <span style="color:green;">BuildQuery</span>.GetObject<<span style="color:blue;">Aluno</span>>(x => x.alunoID == 4);
 </code>
</pre>
<p></br></p>
<h3>GET Object List from database</h3>
<h1></h1>
<p>This is strongly typed function that builds <strong>SELECT</strong> query by object and takes lambda expression as argument to give the user the power of intelicense into his hands.</p>
<pre>
 <code >
// select
   aluno = <span style="color:green;">BuildQuery</span>.GetObjectList<<span style="color:blue;">Aluno</span>>(x => x.nota == "10");
 </code>
</pre>
<p></br></p>
<h3>Notes</h3>
<h1></h1>
<p>This library olds transactions too, just need to set it on initialization in this property:</p>
<pre>
 <code >
 <span style="color:green;">BuildQuery</span>.DbConnection.DbConnectionBase.BeginTransaction
 </code>
</pre>
<p></br></p>
<h3>Requirements</h3>
<h1></h1>
<p><strong>ADO.Lite</strong> requires .NET framework 4.6 +</p>

</body>
</html>
<!-- This document was created with MarkdownPad, the Markdown editor for Windows (http://markdownpad.com) -->
