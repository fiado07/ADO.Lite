
using ADO.Lite.Contracts;
using System.Data;

namespace ADO.Lite.Common
{
    /// <summary>
    /// Connection Base Class
    /// </summary>
    public class DBConnection
    {

        private IDbConnectionSql Connection { get; set; }

        public DBConnection(IDbConnectionSql connection)
        {
            Connection = connection;
        }
        

    }
}
