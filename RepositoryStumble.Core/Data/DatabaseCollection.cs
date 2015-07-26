using System;
using SQLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryStumble.Core.Data
{
    public class DatabaseCollection<TObject, TKey> : IEnumerable<TObject> where TObject : IDatabaseItem<TKey>, new()
    {
        protected SQLiteConnection SqlConnection { get; private set; }

        public TObject this[TKey id]
        {
            get
            {
                return SqlConnection.Find<TObject>(x => x.Id.Equals(id));
            }
        }

        public DatabaseCollection(SQLite.SQLiteConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            SqlConnection.CreateTable<TObject>();
        }

        public virtual void Insert(TObject o)
        {
            SqlConnection.Insert(o);
        }

        public virtual void Update(TObject o)
        {
            SqlConnection.Update(o);
        }

        public virtual void Remove(TObject o)
        {
            SqlConnection.Delete(o);
        }

        public int Count()
        {
            return Query.Count();
        }

        public int Count (Expression<Func<TObject, bool>> predExpr)
        {
            return Query.Count(predExpr);
        }

        public TableQuery<TObject> Query
        {
            get { return SqlConnection.Table<TObject>(); }
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return Query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}