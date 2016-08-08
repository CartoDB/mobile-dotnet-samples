using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;

namespace CartoMobileSample
{
	static class Extensions
	{
		public static string[] ToStringArray(this List<Type> list)
		{
			string[] sampleNames = new string[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				sampleNames[i] = list[i].Name;
			}

			return sampleNames;
		}

		public static ICharSequence ToCharSequence(this string text)
		{
			return new Java.Lang.String(text);
		}

		public static string GetTitle(this Type type)
		{
			return type.Name.Replace("Activity", "");
		}

		public static string GetDescription(this Type type)
		{
			try
			{
				IEnumerable<System.Reflection.CustomAttributeData> list = type.CustomAttributes;
				string description = (string)list.ToList()[1].NamedArguments[0].TypedValue.Value;

				return description;
			} catch {
				return "";
			}
		}

		public static long ToLong(this ulong ulongValue)
		{
			return (long)ulongValue;
		}

		public static string ConvertFromSecondsToHours(this double sec)
		{
			int hours = ((int)sec) / 3600,

			remainder = ((int)sec) % 3600,
			minutes = remainder / 60,
			seconds = remainder % 60;

			return ((hours < 10 ? "0" : "") + hours
				+ "h" + (minutes < 10 ? "0" : "") + minutes
				+ "m" + (seconds < 10 ? "0" : "") + seconds + "s");
		}

		public static void MakeToast(this Android.App.Activity activity, string message)
		{
			activity.RunOnUiThread(delegate {
				Android.Widget.Toast.MakeText(activity, message, Android.Widget.ToastLength.Short).Show();	
			});
		}

	}
}

