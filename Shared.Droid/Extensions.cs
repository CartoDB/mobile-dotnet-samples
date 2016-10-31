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
		public static bool IsHeader(this Type type)
		{
			// Maps have two CustomAttributes (Activity and ActivityData), Headers just one (ActivityData)
			return type.CustomAttributes.ToList().Count == 1;
		}

		public static string GetTitle(this Type type)
		{
			return GetAnnotation(type, 0);
		}

		public static string GetDescription(this Type type)
		{
			return GetAnnotation(type, 1);
		}

		public static string GetAnnotation(Type type, int index)
		{
			try
			{
				IEnumerable<System.Reflection.CustomAttributeData> list = type.CustomAttributes;

				if (list.ToList().Count == 1)
				{
					// It's a header, only one custom attribute
					return (string)list.ToList()[0].NamedArguments[index].TypedValue.Value;
				}

				return (string)list.ToList()[1].NamedArguments[index].TypedValue.Value;
			}
			catch
			{
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

