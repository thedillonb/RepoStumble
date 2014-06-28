using SQLite;

namespace RepositoryStumble.Core.Data
{
	public class InterestedRepository : Repository
    {
		[Indexed]
		public int InterestId { get; set; }

		public StumbledRepository CreateStumbledRepository()
		{
			return new StumbledRepository { Name = Name, Description = Description, Owner = Owner, Stars = Stars, Forks = Forks, Fullname = string.Format("{0}/{1}", Owner, Name) };
		}
    }
}

