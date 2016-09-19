
using System;
using Android.App;
using Carto.Core;
using Carto.Layers;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityDescription(Description ="Events for clicks on base map features")]
	public class InteractivityMapActivity : VectorBaseMapActivity
	{
		VectorLayer VectorLayer { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapPos berlin = new MapPos(24.650415, 59.428773);
			MapView.AnimateZoomTo(berlin);
		}

		protected override void UpdateBaseLayer()
		{
			base.UpdateBaseLayer();

			MapView.InitializeVectorLayer(VectorLayer);
		}
	}
}

