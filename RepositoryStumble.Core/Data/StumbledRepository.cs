using System;
using SQLite;

namespace RepositoryStumble.Core.Data
{
	public class StumbledRepository : Repository
	{
		public bool? Liked { get; set; }

        [Indexed]
		public DateTime CreatedAt { get; set; }

		public bool ShowInHistory { get; set; }

		public StumbledRepository()
		{
			ShowInHistory = true;
		}
	}
}

