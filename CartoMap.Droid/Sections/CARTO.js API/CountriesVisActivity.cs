
using System;
using Android.App;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Countries Vis", Description = "Vis displaying countries in different colors using UTFGrid")]
	public class CountriesVisActivity : BaseVisActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			UpdateVis("http://documentation.cartodb.com/api/v2/viz/2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json");

			// Train: 
			// UpdateVis"(https://mamataakella.cartodb.com/api/v2/viz/30730478-bbb5-11e5-b75c-0e5db1731f59/viz.json");

			// Predicted store location: 
			//UpdateVis("https://maps-for-all.cartodb.com/api/v2/viz/78b33d4a-3dd6-11e6-8632-0ea31932ec1d/viz.json");

			// Telco:
			//UpdateVis("https://raw.githubusercontent.com/CartoDB/labs-telco-insights/gh-pages/telco.viz.json");
		}
	}
}

