using Xamarin.Utilities.Core.Persistence;

namespace RepositoryStumble.Core.Data
{
    public class Interests : DatabaseCollection<Interest, int>
    {
        public Interests(SQLite.SQLiteConnection sqlConnection)
            : base(sqlConnection)
        {
        }
    }
}

