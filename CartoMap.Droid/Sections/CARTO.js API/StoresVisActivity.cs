
using System;
using Android.App;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Predicted Store Location", Description = "Vis showing store locations on the map using UTFGrid")]
	public class DotsVisActivity : BaseVisActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			UpdateVis("https://maps-for-all.cartodb.com/api/v2/viz/78b33d4a-3dd6-11e6-8632-0ea31932ec1d/viz.json");
		}
	}
}

