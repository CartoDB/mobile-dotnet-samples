using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content.Res;
using Java.Lang;

namespace Shared.Droid
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

		public static void CopyAssetToSDCard(this AssetManager manager, string fileName, string writePath)
		{
			using (Stream input = manager.Open(fileName))
			{
				using (var output = new FileStream(writePath, FileMode.Create, FileAccess.Write))
				{
					input.CopyTo(output);
				}
			}
		}

	}
}

