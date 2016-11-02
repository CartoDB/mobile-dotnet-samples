using System;
using Android.App;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityData(Title = "Fonts", Description = "Vis displaying text on the map")]
	public class FontsVisActivity : BaseVisActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			UpdateVis("https://cartomobile-team.carto.com/u/nutiteq/api/v2/viz/13332848-27da-11e6-8801-0e5db1731f59/viz.json");
		}
	}
}

