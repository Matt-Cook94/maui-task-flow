using SQLite;

namespace TaskFlow
{
    public class LocalDbService
    {
        public const string DB_Name = "TaskFlow.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        private readonly SQLiteAsyncConnection _connection;

        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DB_Name);

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(DatabasePath);

            //_connection.CreateTableAsync<TaskItem>();
            //_connection.CreateTableAsync<ListItem>();
        }
    }
}
