using System;
using UIKit;

namespace CartoMobileSample
{
	public class MapListController : UIViewController
	{
		MapListView ContentView { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView = new MapListView();
			View = ContentView;

			ContentView.AddRows(Samples.List);

			Title = "CARTO Mobile Samples";
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			ContentView.DataSource.MapSelected += OnMapSelected;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			ContentView.DataSource.MapSelected -= OnMapSelected;
		}

		void OnMapSelected(object sender, ControllerEventArgs e)
		{
			NavigationController.PushViewController(e.Controller, true);
		}
	}
}

