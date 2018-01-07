using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace EFClassGenerator.Data
{
    internal class DbModelList
    {

        internal delegate DataTable OpenConnectionAndGetTable(string sqlStatment);
        internal delegate DataTable GetTable(SqlConnection connection, string sqlStatment);


        internal string State { get; private set; } = "Ready";
        internal bool TableOk { get; private set; } = false;
        internal List<string> ServerModels { get; private set; }

        internal DbModelList() {}


        internal void GetModels(SqlConnection connection, GetTable cbGetTable)
        {
            var models = cbGetTable(connection, GetSqlStatement());
            ConvertTabelToList(models);
        }

        internal void GetModels(OpenConnectionAndGetTable cbGetTable)
        {
            var models = cbGetTable(GetSqlStatement());
            ConvertTabelToList(models);
        }


        private void ConvertTabelToList(DataTable models)
        {
            var firstColumnName = models.Columns[0].ColumnName;

            ServerModels = models?.AsEnumerable()
                .Select(r => r.Field<string>(firstColumnName))
                .ToList();
            TableOk = (ServerModels != null);

        }


        private string GetSqlStatement()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT");
            sb.AppendLine("    d.name AS [name]");
            sb.AppendLine("FROM");
            sb.AppendLine("    sys.databases AS d ");
            sb.AppendLine("    INNER JOIN sys.master_files AS m ON d.database_id = m.database_id");
            sb.AppendLine("WHERE");
            sb.AppendLine("    d.state_desc = 'ONLINE'");
            sb.AppendLine("    AND m.type_desc = 'ROWS'");
            sb.AppendLine("AND m.name not in ('master','tempdev','tempdb','modeldev','MSDBData')");
            sb.AppendLine("ORDER BY");
            sb.AppendLine("    m.name;");

            return sb.ToString();
        }

    }
}
