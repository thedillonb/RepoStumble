using SQLite;

namespace RepositoryStumble.Core.Data
{
	public class InterestedRepository : Repository
    {
		[Indexed]
		public int InterestId { get; set; }
    }
}

