using System;
using Xamarin.Utilities.Core.Persistence;
using SQLite;
using System.Linq;

namespace RepositoryStumble.Core.Data
{
    public class StumbledRepositories : DatabaseCollection<StumbledRepository, int>
    {
        public StumbledRepositories(SQLiteConnection db)
            : base(db)
        {
        }

        public override void Insert(StumbledRepository o)
        {
            o.CreatedAt = DateTime.Now;
            o.Fullname = (o.Fullname ?? string.Empty).ToLower();
            base.Insert(o);
        }

        public void DeleteAll()
        {
            var deleteItems = this.Where(x => x.Liked == null).ToList();
            foreach (var k in deleteItems)
                Remove(k);
        }

        public void MarkAllAsNotInHistory()
        {
            foreach (var item in this)
            {
                item.ShowInHistory = false;
                Update(item);
            }
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

