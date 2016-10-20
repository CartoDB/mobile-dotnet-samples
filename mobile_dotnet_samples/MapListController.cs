
using System;
using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace Shared.iOS
{
	public class MapListController : UIViewController
	{
		MapListView ContentView { get; set; }

		List<MapListRowSource> sources;

		public MapListController(string title, List<MapListRowSource> sources)
		{
			Title = title;

			this.sources = sources;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView = new MapListView();
			View = ContentView;

			ContentView.AddRows(sources);
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

