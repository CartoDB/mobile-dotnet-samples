﻿using System;
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

	}
}

