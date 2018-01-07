using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ClassGenerator;
using ClassGenerator.Data;
using ClassGenerator.Tools;
using EFClassGenerator.Data;
using EFClassGenerator.Helpers;
using EFClassGenerator.Tools;


/*
 * Code First Relationships Fluent API
    http://msdn.microsoft.com/en-us/data/hh134698.aspx
 * 
 * Creating a More Complex Data Model for an ASP.NET MVC Application
    http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/creating-a-more-complex-data-model-for-an-asp-net-mvc-application
 * 
 * Associations in EF Code First: Part 1 to 5
    http://weblogs.asp.net/manavi/archive/2011/05/01/associations-in-ef-4-1-code-first-part-5-one-to-one-foreign-key-associations.aspx
 * 
 */



namespace EFClassGenerator
{
    public partial class Form1 : Form
    {
        #region Private Properties

        private readonly DbConnect _dbConnect = null;
        private readonly MsSqlServerList _msSqlServerList = null;
        private readonly Worker _worker;

        #endregion Private Properties


        #region Init

        public Form1()
        {
            InitializeComponent();
            lblMessage.Text = @"Searching for SQL - server ...";
            _msSqlServerList = new MsSqlServerList();
            _dbConnect = new DbConnect();
            //var bindingSourceServer = new BindingSource {DataSource = _msSqlServerList.Server};
            //comboBoxHosts.DataSource = bindingSourceServer;
            comboBoxHosts.DataSource = _msSqlServerList.Server;
            _worker = new Worker(_dbConnect);
            lblMessage.Text = @"Ready";

        }

        #endregion Init



        #region Button Events

        private void btnServerListRefresh_MouseUp(object sender, MouseEventArgs e)
        {
            SetState(@"Searching for SQL - server ...");
            _msSqlServerList.RefreshServerList();
            RefreshCombobox(comboBoxHosts, _msSqlServerList.Server);
            SetState(@"Ready");
        }

        private void butCheckConnection_MouseUp(object sender, MouseEventArgs e)
        {
            SetState(@"Testing Connection ...");
            SetConnectionData(false);
            RefreshCombobox(comboBoxModels, _worker.CheckConnectionAndGetModels(SetState));
            
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            SetState(@"Connecting to server...");
            SetConnectionData(true);
            var tables = _worker.CheckConnectionAndGetTables(SetState);
            SetTableContent(tables);
/*            if (!string.IsNullOrWhiteSpace(txtHost.Text) && !string.IsNullOrWhiteSpace(txtUsername.Text)
                && !string.IsNullOrWhiteSpace(txtPassword.Text) && !string.IsNullOrWhiteSpace(txtDatabase.Text))
            {

                if (string.IsNullOrWhiteSpace(txtNamespaceClasses.Text) || string.IsNullOrWhiteSpace(txtNamespaceConfig.Text))
                {
                    lblMessage.Text = "Add the Namspace of Mapping and Configuration";
                }
                else
                {

                    _dbConnect.Connect(txtHost.Text, txtUsername.Text, txtPassword.Text, txtDatabase.Text);
                    DataTable dt = _dbConnect.OpenConnectionAndExecute(SQLQuery.GetTablesAndSynonyms);

                    if (dt != null)
                    {
                        clbTables.DataSource = dt;
                        clbTables.DisplayMember = "name";
                        clbTables.ValueMember = "object_id";
                    }
                    else
                    {
                        lblMessage.Text = "There are some problem with the connection string." + _dbConnect.ErrorMessage;
                    }
                }
            }
            else
            {
                lblMessage.Text = "We need the Host, Username, Password and Database, to connect with DB. Please insert this parameters.";
            } */
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "starting...";
            List<ObjectDB> listTables = new List<ObjectDB>();
            List<ObjectDB> listSynonyms = new List<ObjectDB>();

            //  Cambiar esto. Por ahora solo Tablas
            foreach (DataRowView item in clbTables.CheckedItems)
	        {
                if (item != null)
                {
                    object typeObj = item.Row["type"];
                    if(typeObj != null)
                    {
                        string tmp = typeObj.ToString().Trim();
                        if(tmp== "U")
                        {
                            ObjectDB tmpO = new ObjectDB(item);
                            listTables.Add(tmpO);
                        }
                        else if(tmp== "SN"){
                            ObjectDB tmpO = new ObjectDB(item);
                            listSynonyms.Add(tmpO);
                        }
                    }
                }
	        }

            if (listTables.Count > 0)
                SearchColumnsByTables(listTables);

            if (listSynonyms.Count > 0)
                SearchSynonyms(listSynonyms);

            MessageBox.Show("The process it's end.");
            lblMessage.Text = "done";
            
        }

        #endregion Button Events

        #region Other Controll Events

        private void cbWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = !cbWindowsAuthentication.Checked;
            txtPassword.Enabled = !cbWindowsAuthentication.Checked;
        }


        #endregion 

        #region Private Methods


        private void SetConnectionData(bool setCatalog)
        {
            _worker.Server = GetTextFromComboBox(comboBoxHosts);
            _worker.User = txtUsername.Text;
            _worker.Password = txtPassword.Text;
            _worker.UseWindowsAuthentication = cbWindowsAuthentication.Checked;
            _worker.Catalog = setCatalog ? GetTextFromComboBox(comboBoxModels) : null;
            
        }

        private string GetTextFromComboBox(ComboBox combobox)
        {
            return string.IsNullOrEmpty(combobox.Text) ? combobox.SelectedValue?.ToString() : combobox.Text;
        }

        private void SetState(string state)
        {
            lblMessage.Text = state;
            this.Refresh();

        }

        private void RefreshCombobox(ComboBox comboBox, List<string> items)
        {
            comboBox.DataSource = null;
            comboBox.DataSource = items;
            comboBox.SelectedIndex = comboBox.Items.Count -1;

        }

        private void SetTableContent(DataTable dt)
        {
            if (dt == null) return;
            clbTables.DataSource = dt;
            clbTables.DisplayMember = "name";
            clbTables.ValueMember = "object_id";
        }

        /// <summary>
        /// Search all columns by Table
        /// </summary>
        /// <param name="listTables">Tables to read</param>
        private void SearchColumnsByTables(List<ObjectDB> listTables)
        {
            foreach (var item in listTables)
            {
                DataTable dt = _dbConnect.OpenConnectionAndExecute(string.Format(SQLQuery.GetTablesByName, item.name));

                CreateFile("Classes", item.name + ".cs", GenerateClass(item, dt));
                CreateFile("Mappers", item.name + "Config.cs", GenerateMapper(item, dt));
            }
        }

        /// <summary>
        /// Search all columns by Synonyms Table
        /// </summary>
        /// <param name="listSynonyms">Tables to read</param>
        private void SearchSynonyms(List<ObjectDB> listSynonyms)
        {
            foreach (var item in listSynonyms)
            {
                // get path from Synonym, to search in the real table.
                DataTable dt = _dbConnect.OpenConnectionAndExecute(string.Format(SQLQuery.GetSynonymsByName, item.name));
                if (dt.Rows.Count == 1)
                {
                    string fullPathDb = dt.Rows[0]["base_object_name"].ToString();
                    string dbPath = fullPathDb.Replace(string.Format(".[dbo].[{0}]", item.name), "");

                    //  Get Table
                    dt = _dbConnect.OpenConnectionAndExecute(string.Format("SELECT * FROM {0}.sys.objects WHERE name ='{1}'", dbPath, item.name));
                    item.object_id = int.Parse(dt.Rows[0]["object_id"].ToString());

                    //  Get Columns
                    dt = _dbConnect.OpenConnectionAndExecute(string.Format("SELECT * FROM {0}.INFORMATION_SCHEMA.columns WHERE TABLE_NAME='{1}'", dbPath, item.name));

                    CreateFile("Classes", item.name + ".cs", GenerateClass(item, dt, dbPath + "."));
                    CreateFile("Mappers", item.name + "Config.cs", GenerateMapper(item, dt, dbPath + "."));
                    
                }
                else
                {
                    //  Names duplicated ?
                    lblMessage.Text = "There are Synonyms tables with the same name.";
                }
            }
        }

        /// <summary>
        /// Get .Net property reference
        /// Convert "SQL properties" to ".Net properties"
        /// </summary>
        /// <param name="type">SQL property name</param>
        /// <returns>.net property name</returns>
        private string GetNetType(string type)
        {
            string tmp = type.Trim().ToLower();
            switch (type.ToString().Trim().ToLower())
            {
                case "bigint":
                case "int":
                    return "int";

                case "nchar":
                case "nvarchar":
                case "varchar":
                case "char":
                case "text":
                    return "string";

                case "datetime":
                    return "DateTime";

                case "bit":
                    return "bool";
            }
            return tmp;
        }

        /// <summary>
        /// Create/save File
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="value"></param>
        private void CreateFile(string folder, string filename, string value)
        {
            string pathFolder = FileManager.GetAppPath + "/" + folder;
            FileManager.CheckExistOrCreate(pathFolder);

            //  Save file
            string pathFile = pathFolder + "/" + filename;
            FileManager.WriteFile(pathFile, value);
        }

        /// <summary>
        /// Convert the first letter in Upper case.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string UppercaseFirst(string s)
        {
            if (s == string.Empty) return s;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Remove last "S" from the name and Uppercase first
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string RemoveLast_S(string s)
        {
            if (s == string.Empty) return s;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);  //  Uppercase

            if ( char.ToLower(a[(a.Length-1)]) == 's')
                return new string(a).Substring(0,a.Length-1);

            return new string(a);
        }

        #endregion Private Methods


        #region SQL Queries

        /// <summary>
        /// Get PK code to add in .Net Files.
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="nameTable"></param>
        /// <returns>Line of code to related Primary Key with the DB/ Se obtiene una linea de codigo, que relaciona la PK en .Net con la DB.</returns>
        private string GetPrimaryKey(string dbPath, string nameTable)
        {
            StringBuilder strResult = new StringBuilder();

            //  Query to get the PK from the table
            string query = @"SELECT KU.table_name as tablename,column_name as primarykeycolumn
                                FROM {0}INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                                INNER JOIN {0}INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                                    ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                                    AND ku.table_name='{1}'
                                ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";


            DataTable dtPk = _dbConnect.OpenConnectionAndExecute(string.Format(query, dbPath, nameTable));
            PkTable pk = null;
            foreach (DataRow item in dtPk.Rows)
            {
                pk = new PkTable(item);

                /*
                    -- NOTE:
                                None = The database does not generate values.
                                Identity = The database generates a value when a row is inserted.
                                Computed = The database generates a value when a row is inserted or updated.
                 * 
                 * url:
                 * http://entityframework.codeplex.com/SourceControl/changeset/view/a5faddeca2be#src/EntityFramework/DataAnnotations/Schema/DatabaseGeneratedOption.cs
                 */
                strResult.Append(string.Format("            HasKey(x => x.{0}).Property(x => x.{0}).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);\r\n", UppercaseFirst(pk.PkColumn)));
            }
            return strResult.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object_id"></param>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private DataTable GetFkRelationShips(int object_id, string dbPath = "")
        {
            /*
            if (dbPath != string.Empty)
                dbPath += ".";
            */
            string queryFkRelations = "SELECT fk.name 'FkName' ,columnFirst.name 'FkColumn' ,o.name 'PkTable'	,columnRef.name AS 'PkName'"
                    + " FROM {0}sys.foreign_key_columns fkc"
                    + " INNER JOIN {0}sys.objects AS o ON fkc.referenced_object_id = o.object_id"
                    + " INNER JOIN {0}sys.foreign_keys AS fk ON fkc.constraint_object_id = fk.object_id"
                    + " INNER JOIN {0}sys.columns AS columnFirst ON fkc.parent_object_id = columnFirst.object_id AND fkc.parent_column_id = columnFirst.column_id"
                    + " INNER JOIN {0}sys.columns AS columnRef ON fkc.referenced_object_id = columnRef.object_id AND fkc.referenced_column_id = columnRef.column_id"
                    + " WHERE fkc.parent_object_id = {1}";

            DataTable dtFkRel = _dbConnect.OpenConnectionAndExecute(string.Format(queryFkRelations, dbPath, object_id));
            return dtFkRel;
        }

        private enum TypeSearchRelationship
        {
            Direct,
            Indirect,
            ManyToMany
        }

        private FkTable[] GetFkRelationShips(int object_id, TypeSearchRelationship relationship, string dbPath = "")
        {
            string queryFormat = "";

            if (relationship == TypeSearchRelationship.Direct)
            {
                string query = @"SELECT  
                                            fk.name AS 'FkName',
	                                        o1.name 'FkTable',
	                                        c1.name 'FkColumn',
	                                        o2.name 'PkTable',
	                                        c2.name 'PkColumn'
                                        FROM 
                                            {0}sys.foreign_keys fk
                                        INNER JOIN 
                                            {0}sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id

                                        --  Obtenemos la relacion con las columans
                                        INNER JOIN
                                            {0}sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
                                        INNER JOIN
                                            {0}sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id

                                        --  Obtenemos la relacion con las Tablas
                                        INNER JOIN
	                                        {0}sys.objects o1 ON o1.object_id = c1.object_id
                                        INNER JOIN
	                                        {0}sys.objects o2 ON o2.object_id = c2.object_id
                                        
                                        WHERE fk.{1} = {2}";
                queryFormat = string.Format(query, dbPath, "parent_object_id", object_id);
            }
            else if (relationship == TypeSearchRelationship.Indirect)
            {
                queryFormat = string.Format(SQLQuery.GetOneToMany, object_id, dbPath);
            }
            else if (relationship == TypeSearchRelationship.ManyToMany)
            {
                queryFormat = string.Format(SQLQuery.GetManyToMany, object_id, dbPath);
            }

            var dtFkRel = _dbConnect.OpenConnectionAndExecute(queryFormat) ?? new DataTable();

            int tmpPos = 0;
            FkTable[] fkTables = new FkTable[dtFkRel.Rows.Count];


            FkTable tmpFk = null;

            foreach (DataRow item in dtFkRel.Rows)
            {
                tmpFk = new FkTable(item);
                fkTables[tmpPos] = tmpFk;
                tmpPos++;
            }

            return fkTables;
        }

        #endregion SQL Queries



        #region Base Classe

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectDB"></param>
        /// <param name="list"></param>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private string GenerateClass(ObjectDB objectDB, DataTable list, string dbPath = "")
        {
            string fileStr = FileManager.ReadFile(FileManager.GetAppPath + "/Template/TemplateClass.cs");

            StringBuilder tmpProperties = new StringBuilder();
            StringBuilder tmpRelations = new StringBuilder();
            StringBuilder tmpConstructRelations = new StringBuilder();

            //  Properties
            TableColumns tableColumns = null;
            foreach (DataRow row in list.Rows)
            {
                tmpProperties.Append("\r\n");
                tableColumns = new TableColumns(row);
                tmpProperties.Append(string.Format(@"        public {0}{4} {1} {2} get; set; {3}", GetNetType(tableColumns.Type), UppercaseFirst(tableColumns.ColumnName), "{", "}", tableColumns.IsNullable ? "?" : ""));
                
            }

            FkTable[] fkList = GetFkRelationShips(objectDB.object_id, TypeSearchRelationship.Direct, dbPath);

            //  Related Properties.
            foreach (FkTable item in fkList)
            {
                tmpRelations.Append("\r\n");
                tmpRelations.Append(string.Format(@"        public {0} {3} {1} get; set; {2}", item.PkTableName, "{", "}", RemoveLast_S(item.PkTableName)));
            }


            //  Get Indirect Relationships
            fkList = GetFkRelationShips(objectDB.object_id, TypeSearchRelationship.Indirect, dbPath);
            foreach (FkTable item in fkList)
            {
                tmpRelations.Append("\r\n");
                tmpRelations.Append(string.Format("        public virtual IList<{0}> {0} {1} get; set; {2}\r\n", item.FkTableName, "{", "}"));
                tmpConstructRelations.Append(string.Format("            {0} = new List<{0}>();\r\n", item.FkTableName, "{", "}"));
            }

            //  Many to Many - Tables related
            fkList = GetFkRelationShips(objectDB.object_id, TypeSearchRelationship.ManyToMany, dbPath);
            foreach (FkTable item in fkList)
            {
                tmpRelations.Append(string.Format("        public virtual IList<{0}> {0} {1} get; set; {2}\r\n", item.PkTableName, "{", "}"));
                tmpConstructRelations.Append(string.Format("            {0} = new List<{0}>();\r\n", item.PkTableName, "{", "}"));
            }
            
            //----------------------------------------------------------------------------------------------



            //  Relationships
            fileStr = fileStr.Replace("{0}", txtNamespaceClasses.Text);               //  Namespace
            fileStr = fileStr.Replace("{1}", objectDB.name);                             //  Class Name
            fileStr = fileStr.Replace("{2}", tmpProperties.ToString());         //  Properties  //GetPrimaryKey("",name)
            fileStr = fileStr.Replace("{3}", tmpRelations.ToString());          //  Declaration Relaciones
            fileStr = fileStr.Replace("{4}", tmpConstructRelations.ToString()); //  Inicializacion de las Relaciones

            return fileStr;
        }

        #endregion Base Classe



        #region Configuration Classe

        /// <summary>
        /// [EN]    : Generate Mapper Files. Read Template and replace the values to create Class.
        /// [ESP]   : Genera el archivo mappeado. Lee el Template y reemplaza las variables para crear la clase.
        /// </summary>
        /// <param name="objectDB"></param>
        /// <param name="list"></param>
        /// <param name="dbPath"></param>
        /// <returns>File string/ Archivo en string</returns>
        private string GenerateMapper(ObjectDB objectDB, DataTable list, string dbPath = "")
        {
            string fileStr = FileManager.ReadFile(FileManager.GetAppPath + "/Template/TemplateMapper.cs");

            StringBuilder tmpProperties = new StringBuilder();
            foreach (DataRow row in list.Rows)
            {
                tmpProperties.Append(string.Format("     {0}\r\n", GenerateMapperProperties(new TableColumns(row))));
            }

            FkTable[] fkList = GetFkRelationShips(objectDB.object_id, TypeSearchRelationship.Direct, dbPath);
            StringBuilder tmpRelations = new StringBuilder();
            foreach (FkTable item in fkList)
            {
                tmpRelations.Append(string.Format("{0}\r\n", GenerateMapperRelationShips(objectDB.name, item)));
            }

            fkList = GetFkRelationShips(objectDB.object_id, TypeSearchRelationship.ManyToMany, dbPath);
            foreach (FkTable item in fkList)
            {
                if (item.CreateConfig == 1)
                    tmpRelations.Append(string.Format("{0}\r\n", GenerateMapperRelationShips(objectDB.name, item,true)));
            }

            fileStr = fileStr.Replace("{0}", txtNamespaceConfig.Text);
            fileStr = fileStr.Replace("{1}", objectDB.name);
            fileStr = fileStr.Replace("{2}", GetPrimaryKey(dbPath, objectDB.name));
            fileStr = fileStr.Replace("{3}", tmpProperties.ToString());
            fileStr = fileStr.Replace("{4}", tmpRelations.ToString());
            fileStr = fileStr.Replace("{5}", "using " + txtNamespaceClasses.Text + ";" );

            return fileStr;
        }

        /// <summary>
        /// Generate Property to .Net Files.
        /// </summary>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        private string GenerateMapperProperties(TableColumns tableColumns)
        {
            StringBuilder property = new StringBuilder();
            property.Append(string.Format(@"            Property(x => x.{1}).HasColumnName(""{0}"")", tableColumns.ColumnName, UppercaseFirst(tableColumns.ColumnName)));
            if (!tableColumns.IsNullable)
                property.Append(".IsRequired()");

            if (GetNetType(tableColumns.Type) == "string")
                property.Append(".IsUnicode(false)");

            property.Append(";");

            property.Append("\r\n");
            return property.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fkTable"></param>
        /// <param name="m_m"></param>
        /// <returns></returns>
        private string GenerateMapperRelationShips(string name, FkTable fkTable, bool m_m = false)
        {
            StringBuilder relations = new StringBuilder();
            string tmpStr = "";
            
            if (m_m)
            {
                // Many to Many
                if (fkTable.CreateConfig == 0) return "";

                tmpStr = @"            HasMany(x => x.{0})
                            .WithMany(x => x.{1})
                            .Map(m =>
                            {5}
                                m.ToTable(""{2}"");
                                m.MapLeftKey(""{3}"");
                                m.MapRightKey(""{4}"");
                            {6});";

                return string.Format(tmpStr,
                                fkTable.PkTableName,
                                name,
                                fkTable.FkTableName,
                                fkTable.FkColumn,
                                fkTable.PkColumn,
                                "{", "}");
            }
            else
            {
                //  One to Many

                tmpStr = @"            HasRequired(x => x.{0})
                            .WithMany(x => x.{1})
                            .HasForeignKey(d => d.{2});";

                return string.Format(tmpStr,
                                RemoveLast_S(fkTable.PkTableName),
                                name,
                                UppercaseFirst(fkTable.FkColumn));
            }
        }


        #endregion Configuration Classe

     
    }


}
