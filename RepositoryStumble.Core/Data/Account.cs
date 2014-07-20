using System;
using SQLite;
using System.IO;
using Xamarin.Utilities.Core.Services;
using System.Runtime.Serialization;

namespace RepositoryStumble.Core.Data
{
    [DataContract]
    public class Account : IDisposable
    {
        private static readonly string AccountFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "account.json");
        private static readonly string DBFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db.db");

        private readonly Lazy<SQLiteConnection> _db;
        private readonly Lazy<InterestedRepositories> _interestedRepositories;
        private readonly Lazy<StumbledRepositories> _stumbledRepositories;
        private readonly Lazy<Interests> _interests;

        [DataMember]
		public string Username { get; set; }

        [DataMember]
		public string OAuth { get; set; }

        [DataMember]
		public string Fullname { get; set; }

        [DataMember]
		public string AvatarUrl { get; set; }

        [DataMember]
		public bool SyncWithGitHub { get; set; }

		public Account()
		{
			SyncWithGitHub = true;

            _db = new Lazy<SQLiteConnection>(() => new SQLiteConnection(DBFilePath) { Trace = true });
            _interestedRepositories = new Lazy<InterestedRepositories>(() => new InterestedRepositories(Database));
            _stumbledRepositories = new Lazy<StumbledRepositories>(() => new StumbledRepositories(Database));
            _interests = new Lazy<Interests>(() => new Interests(Database));
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

        public void Dispose()
        {
            Database.Dispose();
            File.Delete(AccountFilePath);
            File.Delete(DBFilePath);
        }

		public SQLiteConnection Database
		{
            get { return _db.Value; }
		}

		public Interests Interests
		{
            get { return _interests.Value; }
		}

		public InterestedRepositories InterestedRepositories
		{
            get { return _interestedRepositories.Value; }
		}

		public StumbledRepositories StumbledRepositories
		{
            get { return _stumbledRepositories.Value; }
		}
    }
}

