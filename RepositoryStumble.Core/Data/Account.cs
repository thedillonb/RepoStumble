using System;
using SQLite;
using System.IO;
using Xamarin.Utilities.Core.Services;

namespace RepositoryStumble.Core.Data
{
    public class Account
    {
		private SQLiteConnection _db;
		private Interests _interests;
		private InterestedRepositories _interestedRepositories;
		private StumbledRepositories _stumbledRepositories;

		private static readonly string AccountFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "account.json");
		private static readonly string DBFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db.db");

		public string Username { get; set; }

		public string OAuth { get; set; }

		public string Fullname { get; set; }

		public string AvatarUrl { get; set; }

		public bool SyncWithGitHub { get; set; }

		public Account()
		{
			SyncWithGitHub = true;
		}

		public static Account Load()
		{
			if (!File.Exists(AccountFilePath))
				return null;

			var str = File.ReadAllText(AccountFilePath, System.Text.Encoding.UTF8);
            return IoC.Resolve<IJsonSerializationService>().Deserialize<Account>(str);
		}

		public void Save()
		{
            var obj = IoC.Resolve<IJsonSerializationService>().Serialize(this);
			File.WriteAllText(AccountFilePath, obj, System.Text.Encoding.UTF8);
		}

		public void Unload()
		{
			_db.Dispose();
			_db = null;
			File.Delete(AccountFilePath);
			File.Delete(DBFilePath);
		}

		public SQLiteConnection Database
		{
			get { return _db ?? (_db = new SQLiteConnection(DBFilePath)); }
		}

		public Interests Interests
		{
			get { return _interests ?? (_interests = new Interests(Database)); }
		}

		public InterestedRepositories InterestedRepositories
		{
			get { return _interestedRepositories ?? (_interestedRepositories = new InterestedRepositories(Database)); }
		}

		public StumbledRepositories StumbledRepositories
		{
			get { return _stumbledRepositories ?? (_stumbledRepositories = new StumbledRepositories(Database)); }
		}
    }
}

