using System;
using System.Data;

namespace EFClassGenerator.Data
{
    internal class TableColumns
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int Order { get; set; }
        public bool IsNullable { get; set; }
        public int MaxLength { get; set; }
        public string Type { get; set; }

        public TableColumns(DataRow dr)
        {
            TableName = Convert.ToString(dr["TABLE_NAME"]);
            ColumnName = Convert.ToString(dr["COLUMN_NAME"]);
            Order = int.Parse(Convert.ToString(dr["ORDINAL_POSITION"]));
            IsNullable = Convert.ToString(dr["IS_NULLABLE"]).Trim() != "NO";
            int.TryParse(dr["CHARACTER_MAXIMUM_LENGTH"].ToString(), out var tmpMaxLength);
            MaxLength = tmpMaxLength;
            Type = Convert.ToString(dr["DATA_TYPE"]);
        }
    }
}
