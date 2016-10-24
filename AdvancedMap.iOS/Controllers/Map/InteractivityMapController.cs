
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class InteractivityMapController : VectorMapBaseController
	{
		public override string Name { get { return "Interactivity map"; } }

		public override string Description { get { return "Events for clicks on base map features"; } }

		VectorLayer VectorLayer { get; set; }

		ForceTouchRecognizer recognizer = new ForceTouchRecognizer();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapPos berlin = new MapPos(24.650415, 59.428773);
			MapView.AnimateZoomTo(berlin);

			recognizer = new ForceTouchRecognizer();
			MapView.AddGestureRecognizer(recognizer);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			recognizer.ForceTouch += OnForceTouch;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			recognizer.ForceTouch -= OnForceTouch;
		}

		void OnForceTouch(object sender, iOS.ForceEventArgs e)
		{
			Console.WriteLine("OnForceTouch: " + e.Type);
		}

		protected override void UpdateBaseLayer()
		{
			base.UpdateBaseLayer();

			MapView.InitializeVectorTileListener(VectorLayer);
		}
	}
}

