using SQLite;
using Splat;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.Data
{
    public class InterestedRepositories : DatabaseCollection<InterestedRepository, int>
    {
        public InterestedRepositories(SQLiteConnection db)
            : base(db)
        {
            bool done;
            if (!Locator.Current.GetService<IDefaultValueService>().TryGet("update_interested_repositories", out done) || !done)
            {
                MarkAllLowercase();
                Locator.Current.GetService<IDefaultValueService>().Set("update_interested_repositories", true);
            }
        }

        public void MarkAllLowercase()
        {
            foreach (var item in this.Query)
                Update(item);
        }

        public override void Insert(InterestedRepository o)
        {
            o.Fullname = (o.Fullname ?? string.Empty).ToLower();
            base.Insert(o);
        }

        public override void Update(InterestedRepository o)
        {
            o.Fullname = (o.Fullname ?? string.Empty).ToLower();
            base.Update(o);
        }
    }
}

