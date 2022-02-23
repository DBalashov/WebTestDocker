using System;

namespace Handler
{
    public class DatabaseHandlerStub : IDatabaseHandler
    {
        public bool Enabled => false;

        public void PrepareDatabase()
        {
        }

        public void Put(DBLogItem item)
        {
        }

        public DBLogItem[] GetLast(int n) => Array.Empty<DBLogItem>();
    }
}