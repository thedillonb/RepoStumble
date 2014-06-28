using SQLite;
using Xamarin.Utilities.Core.Persistence;

namespace RepositoryStumble.Core.Data
{
    public class Repository : IDatabaseItem<int>
    {
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

        public string Owner { get; set; }

		public string Name { get; set; }

		[Indexed]
		public string Fullname { get; set; }

		[MaxLength(2048)]
        public string Description { get; set; }

		public uint Stars { get; set; }

		public uint Forks { get; set; }
    }
}

