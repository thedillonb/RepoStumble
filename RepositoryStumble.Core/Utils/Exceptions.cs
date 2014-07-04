using System;

namespace RepositoryStumble.Core.Utils
{
	public class InterestExhaustedException : Exception
    {
		public InterestExhaustedException()
			: base("There is nothing more to see for this interest! You've seen it all!")
		{
		}
    }
}

