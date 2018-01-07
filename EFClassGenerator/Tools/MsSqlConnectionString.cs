using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace EFClassGenerator.Tools
{
    internal class MsSqlConnectionString
    {
        private readonly MsSqlConcectionData _conData;

        internal string Generate => GenerateConnectionString();

        internal MsSqlConnectionString (MsSqlConcectionData concectionData )
        {
            this._conData = concectionData;

        }

        private string GenerateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder {DataSource = _conData.Server};

            if (_conData.UseWindowsAuthentication)
            {
                builder.IntegratedSecurity = true;
            } else
            {
                builder.UserID = _conData.User;
                builder.Password = _conData.Password;
                builder.IntegratedSecurity = false;
            }

            builder.InitialCatalog = _conData.Catalog;

            return builder.ConnectionString;
        }

    }


}
