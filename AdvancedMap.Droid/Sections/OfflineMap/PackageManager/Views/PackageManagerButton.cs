
using System;
using Android.Widget;
using Shared;

namespace AdvancedMap.Droid
{
	public class PackageManagerButton : Button
	{
		public string PackageId { get; set; }

		public string PackageName { get; set; }

		public int PriorityIndex { get; set; }

		public PMButtonType Type { get; set; }

		public PackageManagerButton(Android.Content.Context context) : base(context)
		{

		}

		public PackageManagerButton(Android.Content.Context context, Android.Util.IAttributeSet attrs) : base(context, attrs)
		{
			// TODO Auto-generated constructor stub
		}
	}

}
