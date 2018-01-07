using System;
using System.Data;

namespace EFClassGenerator.Data
{
    class FkTable
    {
        public string FkName { get; set; }

        public string FkTableName { get; set; }
        public string FkColumn { get; set; }


        public string PkColumn { get; set; }
        public string PkTableName { get; set; }

        public int CreateConfig { get; set; }

        public FkTable(DataRow dr)
        {
            FkName = Convert.ToString(dr["FkName"]);
            FkColumn = Convert.ToString(dr["FkColumn"]);
            FkTableName = Convert.ToString(dr["FkTable"]);

            PkColumn = Convert.ToString(dr["PkColumn"]);
            PkTableName = Convert.ToString(dr["PkTable"]);

            CreateConfig = 0;

            if (dr.Table.Columns.Contains("CreateConfig"))
                CreateConfig = int.Parse(Convert.ToString(dr["CreateConfig"]));

 
        }

    }
}
