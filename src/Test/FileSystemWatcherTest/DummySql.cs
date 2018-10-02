using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FileSystemWatcherTest
{
    public class DummySql : ISqlService
    {
        public T GetIdentity<T>(string tableName, string name = "default")
        {
            throw new NotImplementedException();
        }

        public void OpenConnection(Action<IDbConnection> action, string name = "default")
        {
            throw new NotImplementedException();
        }

        public T OpenConnection<T>(Func<IDbConnection, T> action, string name = "default")
        {
            throw new NotImplementedException();
        }
    }

    public class DummyColumnService : ISqlColumnService
    {
        public IDictionary<string, string> GetModelDBColumnNames(string tableName, Type modelType, IDictionary<string, string> differentColumnName)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetModelDBColumnNames(string tableName, Type modelType, IDictionary<string, string> differentColumnName, string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}
