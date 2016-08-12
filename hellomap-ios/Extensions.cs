
using System;

namespace CartoMobileSample
{
	public static class Extensions
	{
		public static string Append(this string url, string key, string value)
		{
			string property = "";

			if (url[url.Length - 1] != '?')
			{
				property += "&";
			}

			property += key + "=" + value;

			return url + property;
		}
	}
}

