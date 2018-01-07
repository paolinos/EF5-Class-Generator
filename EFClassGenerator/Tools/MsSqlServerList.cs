using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Runtime.CompilerServices;
using EFClassGenerator.Annotations;

namespace EFClassGenerator.Tools
{
    internal class MsSqlServerList : INotifyPropertyChanged
    {

        private readonly SqlDataSourceEnumerator _sqlServerInstances;

        internal List<string> Server { get; } = new List<string>();

        internal MsSqlServerList()
        {
            _sqlServerInstances = SqlDataSourceEnumerator.Instance;
            GetServerList();
        }


        internal void RefreshServerList()
        {
            GetServerList();
        }
        private void GetServerList()
        {
            var serverTable = _sqlServerInstances.GetDataSources();
            Server.Clear();
            const char seperator = '\\';

            if (serverTable.Rows.Count <=0) return;

            var serverTableColumns = GetColumns(serverTable);

            foreach (DataRow row in serverTable.Rows)
            {
                var server = row[serverTableColumns[0]].ToString();
                var instance = row[serverTableColumns[1]].ToString();
                if (instance.Length > 0)
                {
                    server += seperator + instance;
                }

                Server.Add(server);
            }
            OnPropertyChanged(nameof(Server));
        }

        private List<DataColumn> GetColumns(DataTable serverTable)
        {
            return serverTable.Columns.Cast<DataColumn>().ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}