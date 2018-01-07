using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using ClassGenerator.Data;
using EFClassGenerator.Data;
using EFClassGenerator.Tools;

namespace EFClassGenerator.Helpers
{
    internal class Worker

    {
        private readonly DbConnect _dbConnect;

        internal delegate void SetState(string state);
        internal string Server { private get; set; }
        internal string User { private get; set; }
        internal string Password { private get; set; }
        internal bool UseWindowsAuthentication { private get; set; }
        internal string Catalog { private get; set; }
        

        internal Worker(DbConnect dbConnect)
        {
            this._dbConnect = dbConnect;
        }


        internal List<string> CheckConnectionAndGetModels(SetState setState)
        {
            var connectionData = GetConnectionData(false);
            connectionData.CheckServerData();

            if (!connectionData.ServerDataComplete)
            {
                setState(connectionData.State);
                return null;
            }

            _dbConnect.ConnectionString = connectionData.ConnectionString;
            var models = _dbConnect.CheckConnectionStringAndGetModels();

            var state = _dbConnect.ConnectionStringOk ? @"DB - Connection OK" : _dbConnect.ErrorMessage;
            if (_dbConnect.ConnectionStringOk && models == null) state += @" - No Models Found";

            setState(state);
            return models;

        }


        internal DataTable CheckConnectionAndGetTables(SetState setState)
        {
            var connectionData = GetConnectionData(true);
            connectionData.CheckCompleteData();

            if (!connectionData.DbDataComplete)
            {
                setState(@"Cannot connect to server: " + connectionData.State);
                return null;
            }

            _dbConnect.ConnectionString = connectionData.ConnectionString;
            var dt = _dbConnect.OpenConnectionAndExecute(SQLQuery.GetTablesAndSynonyms);
            if (dt == null) setState(@"Didn't find any Tabel: " + connectionData.State);
            return dt;
        }

        internal List<string> GetModels(SetState setState)
        {
            if (!_dbConnect.ConnectionStringOk)
            {
                setState(_dbConnect.ErrorMessage);
                return null;
            }

            _dbConnect.GetModels();
            return null;
        }



        private MsSqlConcectionData GetConnectionData(bool withTables)
        {
            var conectionData = new MsSqlConcectionData
            {
                Server = this.Server
                ,User = this.User
                ,Password = this.Password
                ,UseWindowsAuthentication = this.UseWindowsAuthentication
                //,Timeout = 30
            };

            if (withTables) conectionData.Catalog = Catalog;

            return conectionData;
        }

    }
}
