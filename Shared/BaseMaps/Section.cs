using System;
using System.Collections.Generic;

namespace Shared
{
	public enum MapType
	{
		None,
		Raster,
		Vector
	}

	public class Section
	{
		public NameValuePair OSM { get; set; }

		public MapType Type { get; set; }

		public List<NameValuePair> Styles { get; set; }

		public override bool Equals(object obj)
		{
			if (!(obj is Section))
			{
				return false;
			}

			Section comparable = (Section)obj;

			return OSM.Value == comparable.OSM.Value;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class NameValuePair 
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
}

