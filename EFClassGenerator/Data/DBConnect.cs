using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EFClassGenerator.Data
{
    public class DbConnect
    {
        private string host = "";
        private string username = "";
        private string passowrd = "";
        private string database = "";
        private string _connectionString;

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                ConnectionStringOk = false;
                _connectionString = value;
            }
        }

        public string ErrorMessage { get; private set; }
        public bool ConnectionStringOk { get; private set; } = false;



        public DbConnect()         {        }



        /// <summary>
        /// Create DB Connect, and set the first parameters to connect with DB.
        /// </summary>
        /// <param name="_host">host / server</param>
        /// <param name="_username">username</param>
        /// <param name="_password">password</param>
        /// <param name="_database">database</param>
        public void Connect(string _host, string _username, string _password, string _database)
        {
            host = _host;
            username = _username;
            passowrd = _password;
            database = _database;
        }

        /// <summary>
        /// Check Connection.
        /// </summary>
        /// <returns></returns>
        public void CheckConnection()
        {
            try
            {
                ErrorMessage = "";
                ConnectionStringOk = false;

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    ConnectionStringOk = connection.State == ConnectionState.Open;
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public List<string> CheckConnectionStringAndGetModels()
        {
            ErrorMessage = "";
            ConnectionStringOk = false;
            var modelList = new DbModelList();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    ConnectionStringOk = connection.State == ConnectionState.Open;

                    if (ConnectionStringOk) modelList.GetModels(connection, Execute);

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return modelList.ServerModels;
        }


        public List<string> GetModels()
        {
            var modeList = new DbModelList();
            modeList.GetModels(OpenConnectionAndExecute);
            

            return modeList.ServerModels;
        }


        public DataTable Execute(SqlConnection connection, string query)
        {

            try
            {
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    var sqlData = new DataSet("SqlData");
                    adapter.Fill(sqlData);
                    if (sqlData.Tables.Count == 1) return sqlData.Tables[0];
                    ErrorMessage = @"More than one Tabel returned by query";
                    return null;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }


            return null;
        }

        public DataTable OpenConnectionAndExecute(string query)
        {

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    //ConnectionStringOk = connection.State == ConnectionState.Open;

                    var sqlData = new DataSet("SqlData");
                    adapter.Fill(sqlData);
                    if (sqlData.Tables.Count == 1) return sqlData.Tables[0];

                    ErrorMessage = @"More or less than one Table returned by query";
                    return null;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }

            
            return null;            
        }

        

  
    }
}
