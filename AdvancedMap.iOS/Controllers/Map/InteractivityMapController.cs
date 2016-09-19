
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Shared;

namespace AdvancedMap.iOS
{
	public class InteractivityMapController : VectorMapBaseController
	{
		public override string Name { get { return "Interactivity map"; } }

		public override string Description { get { return "Events for clicks on base map features"; } }

		VectorLayer VectorLayer { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

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

