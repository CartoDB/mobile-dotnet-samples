using System;
namespace Shared
{
	public static class PackageExtensions
	{
		public static bool HasInfo(this Package package)
		{
			return package.Info != null;
		}
	}
}
