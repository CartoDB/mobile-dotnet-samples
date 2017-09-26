
using System;
using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace Shared.iOS
{
	public class MapListController : UIViewController
	{
		MapGalleryView ContentView { get; set; }

		List<Sample> sources;

		public MapListController(string title, List<Sample> sources)
		{
			Title = title;

			this.sources = sources;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView = new MapGalleryView();
			View = ContentView;

			ContentView.AddRows(sources);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			ContentView.RowClick += OnRowClick;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			ContentView.RowClick -= OnRowClick;
		}

		void OnRowClick(object sender, EventArgs e)
		{
			GalleryRow row = (GalleryRow)sender;
			NavigationController.PushViewController(row.Source.Controller, true);
		}

	}
}

