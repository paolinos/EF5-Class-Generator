using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ClassGenerator.Data
{
    public class DBConnect
    {
        private string msgError = "";
        private string host = "";
        private string username = "";
        private string passowrd = "";
        private string database = "";


        public DBConnect()         {        }


        /// <summary>
        /// Get Error Message
        /// </summary>
        /// <returns></returns>
        public string Error        {            get{return msgError;}        }

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
        /// WARNING: Maybe this it's not working.
        /// </summary>
        /// <returns></returns>
        public bool CheckConnection()
        {
            try
            {
                msgError = "";
                SqlConnection connection = new SqlConnection(GetConnectionString());
                //connection.StateChange += connection_StateChange;
                connection.Open();
                connection.Close();
                return true;
            }
            
            catch (SqlException Ex)
            {
                msgError = Ex.Message;
            }
            return false;
        }

        

        /// <summary>
        /// Execute Query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>DataTable result</returns>
        public DataTable Execute(string query)
        {
            if (CheckConnection())
            {
                msgError = string.Empty;
                SqlConnection connection = new SqlConnection(GetConnectionString(240));

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);

                sqlDataAdapter.Dispose();
                connection.Close();

                return dt;
            }
            return null;            
        }

        

        /// <summary>
        /// Connection String
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString(int timeOut = 15)
        {
            return string.Format("Server={0};initial catalog={1};User Id={2};Password={3};Min Pool Size=0;Connect Timeout={4};Connection Lifetime=60;", host, database, username, passowrd, timeOut);
        }

        /// <summary>
        /// State change - DB Event 
        /// </summary>
        private void connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                //  conection closed
            }
        }
    }
}
