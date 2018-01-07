using System;
using System.Data.SqlClient;


namespace EFClassGenerator.Tools

{
    internal class MsSqlConcectionData
    {
        internal string Server { get; set; }
        internal bool UseWindowsAuthentication { get; set; }
        internal string User { get; set; }
        internal string Password { get; set; }
        internal int Timeout { get; set; } = 15;


        internal string Catalog { get; set; }

        internal string State { get; private set; }
        internal bool ServerDataComplete { get; private set; }
        internal bool DbDataComplete { get; private set; }

        internal string ConnectionString => GenerateConnectionString();


        internal MsSqlConcectionData() {}

        internal void CheckServerData()
        {
            ServerDataComplete = CheckServerParameter();
        }

        private bool CheckServerParameter()
        {
            if (string.IsNullOrEmpty(Server))
            {
                State = "No Servername specified!";
                return false;
            }

            if (UseWindowsAuthentication)
            {
                State = "ready";
                return true;
            }

            if (string.IsNullOrEmpty(User))
            {
                State = "Need User to be specified! (Or Windows Authentificaton)";
                return false;

            }

            if (string.IsNullOrEmpty(Password))
            {
                State = "No Password specified!";
                return false;

            }

            State = "ready";
            return true;

        }

        internal void CheckCompleteData()
        {
            DbDataComplete = CheckAllParameter();
        }

        private bool CheckAllParameter()
        {
            if (!string.IsNullOrEmpty(Catalog)) return CheckServerParameter();
            State = "Need Catalog to be specified!";
            return false;
        }


        private string GenerateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder { DataSource = Server };

            if (UseWindowsAuthentication)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = User;
                builder.Password = Password;
                builder.IntegratedSecurity = false;
            }

            builder.ConnectTimeout = Timeout;
            if (!string.IsNullOrEmpty(Catalog)) builder.InitialCatalog = Catalog;

            return builder.ConnectionString;
        }

    }



}

    
