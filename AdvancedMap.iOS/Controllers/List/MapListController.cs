using System;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
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

			ContentView.ListSource.MapSelected += OnMapSelected;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			ContentView.ListSource.MapSelected -= OnMapSelected;
		}

		void OnMapSelected(object sender, ControllerEventArgs e)
		{
			NavigationController.PushViewController(e.Controller, true);
		}
	}
}

