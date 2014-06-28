using System;

namespace RepositoryStumble.Core.Data
{
	public class StumbledRepository : Repository
	{
		public bool? Liked { get; set; }

		public DateTime CreatedAt { get; set; }

		public bool ShowInHistory { get; set; }

		public StumbledRepository()
		{
			ShowInHistory = true;
		}
	}
}

