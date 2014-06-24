using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ClassGenerator.Data
{
    public class ObjectDB
    {
        public string name { get; set; }
        public int object_id { get; set; }
        public int schema_id { get; set; }
        public int parent_object_id  { get; set; }
        public string type { get; set; }
        public string type_desc { get; set; }

        public ObjectDB(DataRowView dataRow)
        {
            name = dataRow["name"].ToString();
            object_id = int.Parse(dataRow["object_id"].ToString());
            schema_id = int.Parse(dataRow["schema_id"].ToString());
            parent_object_id = int.Parse(dataRow["parent_object_id"].ToString());
            type = dataRow["type"].ToString();
            type_desc = dataRow["type_desc"].ToString();
        }

        public bool isSynonym
        {
            get { return type == "SN" ? true : false; }
        }
    }
}
