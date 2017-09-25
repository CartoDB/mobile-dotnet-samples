
using System;
using Android.App;
using Shared.Droid;
using Shared;
using Android.Views;
using Carto.Ui;
using Carto.Layers;
using Carto.Core;
using Carto.VectorTiles;
using Carto.DataSources;
using Carto.Utils;
using Carto.Styles;
using AdvancedMap.Droid.Sections.BaseMap.Views;
using AdvancedMap.Droid.Sections.BaseMap.Subviews;
using Shared.Model;
using Android.Widget;

namespace AdvancedMap.Droid
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	[ActivityData(Title = "Base maps", Description = "Overview of base maps offered by CARTO")]
	public class BaseMapsActivity : BaseActivity
	{
		BaseMapsView ContentView { get; set; }

		MapView MapView { get { return ContentView.MapView; } }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new BaseMapsView(this);
			SetContentView(ContentView);

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

            ContentView.CurrentLayer = ContentView.AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            ContentView.StyleContent.HighlightDefault();
            ContentView.LanguageContent.Adapter.Languages = Languages.List;
            ContentView.LanguageContent.Adapter.NotifyDataSetChanged();
		}

		protected override void OnResume()
		{
			base.OnResume();

            ContentView.BasemapButton.Clicked += OnBasemapButtonClick;
            ContentView.LanguageButton.Clicked += OnLanguageButtonClick;

            foreach (var section in ContentView.StyleContent.Sections)
            {
                foreach (var item in section.List)
                {
                    item.Click += OnStyleItemClick;
                }    
            }

            ContentView.InitializeVectorTileListener();

            ContentView.LanguageContent.List.ItemClick += OnLanguageClick;
		}

        protected override void OnPause()
		{
			base.OnPause();

            ContentView.BasemapButton.Clicked -= OnBasemapButtonClick;
            ContentView.LanguageButton.Clicked -= OnLanguageButtonClick;

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

            ContentView.LanguageContent.List.ItemClick -= OnLanguageClick;

		}

        void OnLanguageClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ContentView.Popup.Hide();

            Language language = ContentView.LanguageContent.Adapter.Languages[e.Position];
            ContentView.UpdateLanguage(language);
        }

        void OnBasemapButtonClick(object sender, EventArgs e)
        {
            ContentView.Popup.SetPopupContent(ContentView.StyleContent);
            ContentView.Popup.Show();
        }

		void OnLanguageButtonClick(object sender, EventArgs e)
		{
            ContentView.Popup.SetPopupContent(ContentView.LanguageContent);
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
            string source = (item.Parent as StylePopupContentSection).Source;
            ContentView.UpdateBaseLayer(selection, source);

            ContentView.StyleContent.Previous = item;
		}

	}
}

