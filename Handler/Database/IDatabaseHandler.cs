namespace Handler
{
    public interface IDatabaseHandler
    {
        bool Enabled { get; }

        void PrepareDatabase();

        void Put(DBLogItem item);

        DBLogItem[] GetLast(int n);
    }
}