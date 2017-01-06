
using System;
using Android.App;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "NYCity Subway Vis", Description = "Vis displaying thes subway in different colors using UTFGrid")]
	public class CountriesVisActivity : BaseVisActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			UpdateVis("https://mamataakella.cartodb.com/api/v2/viz/30730478-bbb5-11e5-b75c-0e5db1731f59/viz.json");
		}
	}
}

