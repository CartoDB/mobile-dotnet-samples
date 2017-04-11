using System;
using System.Collections.Generic;

namespace Shared
{
	public class TorqueUtils
	{
		const int incrementBy = 15;

		public static List<string> timestamps;

		public static bool Initialized { get { return timestamps != null; } }

		public static void Initialize()
		{
			timestamps = new List<string>();

			/*
			 * Hardcoded (pretty) timestamp in accordance with the web UI.
			 * Non-hardcoded currently only available via external api
			 * 
			 * We know the start date, count and interval - just loop over it and create pretty timestamps
			 */
			var date = new DateTime(2016, 9, 15, 12, 14, 0);

			for (int i = 0; i < 256; i++)
			{
				string timestamp = date.ToString("HH:mm dd/MM/yyyy");
				timestamps.Add("  " + timestamp + "  ");
				date = date.AddMinutes(incrementBy);
			}
		}

		public static string GetText(int frameNumber)
		{
			return timestamps[frameNumber];
		}
	}
}
