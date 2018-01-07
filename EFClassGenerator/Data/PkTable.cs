using System;
using System.Data;

namespace EFClassGenerator.Data
{
    internal class PkTable
    {
        public string TableName { get; set; }
        public string PkColumn { get; set; }

        public PkTable(DataRow dr)
        {
            TableName = Convert.ToString(dr["tablename"]);
            PkColumn = Convert.ToString(dr["primarykeycolumn"]);
        }
    }
}
