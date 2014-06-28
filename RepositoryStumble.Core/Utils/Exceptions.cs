using System;

namespace RepositoryStumble
{
	public class InterestExhaustedException : Exception
    {
		public InterestExhaustedException()
			: base("There is nothing more to see for this interest! You've seen it all!")
		{
		}
    }
}

