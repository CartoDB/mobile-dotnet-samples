using System;
namespace Shared
{
	public static class RendererExtensions
	{
		public static int Update(this int original, float ratio)
		{
			return (int)(original * ratio);
		}
	}
}

