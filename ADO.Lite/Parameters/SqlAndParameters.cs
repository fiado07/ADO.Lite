
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
    public class SqlAndParameters
    {
        public string Sql { get; set; }

        public bool isStoredProcedure { get; set; } = false;


        public List<Common.Parameter> Parameter { get; set; }

      
        public SqlAndParameters()
        {

        }



    }
}
