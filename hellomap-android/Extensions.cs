using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;

namespace CartoMobileSample
{
	static class Extensions
	{
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

		public static ICharSequence ToCharSequence(this string text)
		{
			return new Java.Lang.String(text);
		}

		public static void MakeToast(this Android.App.Activity activity, string message)
		{
			activity.RunOnUiThread(delegate {
				Android.Widget.Toast.MakeText(activity, message, Android.Widget.ToastLength.Short).Show();	
			});
		}

	}
}

