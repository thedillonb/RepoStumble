using Xamarin.Utilities.Core.Persistence;
using SQLite;

namespace RepositoryStumble.Core.Data
{
    public class InterestedRepositories : DatabaseCollection<InterestedRepository, int>
    {
        public InterestedRepositories(SQLiteConnection db)
            : base(db)
        {
        }
    }
}

