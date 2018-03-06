
using ADO.Lite.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Lite.Parameters
{
    /// <summary>
    /// Holds sql and sqlParameters
    /// </summary>
    public class SqlParameter
    {
        public string Sql { get; set; }
        public string Predicate { get; set; }
        public List<Common.KeyValue> Parameter { get; set; }

      
        public SqlParameter()
        {

        }



    }
}
