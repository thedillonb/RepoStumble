using SQLite;
using Xamarin.Utilities.Core.Persistence;

namespace RepositoryStumble.Core.Data
{
    public class Interest : IDatabaseItem<int>
    {
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Language { get; set; }

		public string LanguageId { get; set; }

		public string Keyword { get; set; }

		public uint NextPage { get; set; }

		public bool Exhaused { get; set; }

		public override string ToString()
		{
			return string.Format("[Interest: Id={0}, Language={1}, LanguageId={2}, Keyword={3}, NextPage={4}, Exhaused={5}]", Id, Language, LanguageId, Keyword, NextPage, Exhaused);
		}
    }
}

