using System;
using Android.App;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(Label = "")]
	[ActivityDescription(Description = "Choice of different Base Maps")]
	public class BaseMapsActivity : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}
	}
}

