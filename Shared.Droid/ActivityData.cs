
using System;

namespace Shared.Droid
{
	public class ActivityData : Attribute
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public bool IsHeader { get; set; } = false;
	}
}

