
using System;
using AdvancedMap.iOS.Sections.BaseMap;
using AdvancedMap.iOS.Sections.BaseMap.Subviews;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using CoreGraphics;
using Foundation;
using Shared;
using Shared.iOS;
using Shared.Model;
using UIKit;

namespace AdvancedMap.iOS
{
    public class BaseMapsController : BaseController, IUITableViewDelegate
	{
		public override string Name { get { return "Choice of different Base Maps"; } }

		public override string Description { get { return "Overview of base maps offered by CARTO"; } }
		
        BaseMapsView ContentView { get; set; }

		MapView MapView { get { return ContentView.MapView; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView = new BaseMapsView();
            View = ContentView;

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

			ContentView.CurrentLayer = ContentView.AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
			ContentView.StyleContent.HighlightDefault();
            ContentView.LanguageContent.AddLanguages(Languages.List);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

            ContentView.BasemapButton.Click += OnBasemapButtonClick;
            ContentView.LanguageButton.Click += OnLanguageButtonClick;

			foreach (var section in ContentView.StyleContent.Sections)
			{
				foreach (var item in section.List)
				{
					item.Click += OnStyleItemClick;
				}
			}

            ContentView.LanguageContent.Table.Delegate = this;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			ContentView.BasemapButton.Click -= OnBasemapButtonClick;
			ContentView.LanguageButton.Click -= OnLanguageButtonClick;

			foreach (var section in ContentView.StyleContent.Sections)
			{
				foreach (var item in section.List)
				{
					item.Click -= OnStyleItemClick;
				}
			}

			if (ContentView.CurrentLayer is VectorTileLayer)
			{
				(ContentView.CurrentLayer as VectorTileLayer).VectorTileEventListener = null;
			}

            ContentView.LanguageContent.Table.Delegate = null;
		}

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
			ContentView.Popup.Hide();

            Language language = ContentView.LanguageContent.Languages[indexPath.Row];
			ContentView.UpdateLanguage(language);
        }

		void OnBasemapButtonClick(object sender, EventArgs e)
		{
            ContentView.Popup.SetContent(ContentView.StyleContent);
			ContentView.Popup.Show();
		}

		void OnLanguageButtonClick(object sender, EventArgs e)
		{
            ContentView.Popup.SetContent(ContentView.LanguageContent);
			ContentView.Popup.Show();
		}

		void OnStyleItemClick(object sender, EventArgs e)
		{
			ContentView.Popup.Hide();

			if (ContentView.StyleContent.Previous != null)
			{
				ContentView.StyleContent.Previous.Normalize();
			}

			var item = (StylePopupContentSectionItem)sender;
			item.Highlight();

			string selection = item.Label.Text;
            string source = (item.Superview as StylePopupContentSection).Source;
			ContentView.UpdateBaseLayer(selection, source);

			ContentView.StyleContent.Previous = item;
		}
	}
}

