using System;
using SQLite;
using System.Linq;
using Splat;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.Data
{
    public class StumbledRepositories : DatabaseCollection<StumbledRepository, int>
    {
        public StumbledRepositories(SQLiteConnection db)
            : base(db)
        {
            bool done;
            if (!Locator.Current.GetService<IDefaultValueService>().TryGet("update_stumbled_repositories", out done) || !done)
            {
                MarkAllLowercase();
                Locator.Current.GetService<IDefaultValueService>().Set("update_stumbled_repositories", true);
            }
        }

        public override void Insert(StumbledRepository o)
        {
            o.CreatedAt = DateTime.Now;
            o.Fullname = (o.Fullname ?? string.Empty).ToLower();
            base.Insert(o);
        }

        public override void Update(StumbledRepository o)
        {
            o.Fullname = (o.Fullname ?? string.Empty).ToLower();
            base.Update(o);
        }

        public void DeleteAll()
        {
            var deleteItems = this.Query.Where(x => x.Liked == null).ToList();
            foreach (var k in deleteItems)
                Remove(k);
        }

        public void MarkAllAsNotInHistory()
        {
            foreach (var item in this.Query)
            {
                item.ShowInHistory = false;
                Update(item);
            }
        }

        public void MarkAllLowercase()
        {
            foreach (var item in this.Query)
                Update(item);
        }

        public int LikedRepositories()
        {
            return Query.Count(x => x.Liked == true);
        }

        public int DislikedRepositories()
        {
            return Query.Count(x => x.Liked == false);
        }

        public StumbledRepository FindByFullname(string owner, string name)
        {
            var str = string.Format("{0}/{1}", owner, name).ToLower();
            return SqlConnection.Find<StumbledRepository>(x => x.Fullname == str);
        }

        public bool Exists(string owner, string name)
        {
            var str = string.Format("{0}/{1}", owner, name).ToLower();
            return SqlConnection.Find<StumbledRepository>(x => x.Fullname == str) != null;
        }
    }
}

