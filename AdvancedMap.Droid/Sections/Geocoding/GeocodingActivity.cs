using System;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Carto.Geocoding;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    [Activity]
    [ActivityData(Title = "Geocoding", Description = "Type in an address and go to that location")]
    public class GeocodingActivity : BaseGeocodingActivity
    {
        public new GeocodingView ContentView 
        { 
            get { return base.ContentView as GeocodingView; }
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.ContentView = new GeocodingView(this);
            SetContentView(base.ContentView);

            GeocodingClient.Projection = ContentView.Projection;
            GeocodingClient.ApiKey = ApiKey;
            SetOnlineMode();

            Window.SetSoftInputMode(SoftInput.StateHidden | SoftInput.AdjustNothing);
        }

        protected override void OnResume()
        {
            base.OnResume();

            ContentView.Field.TextChanged += OnTextChange;
            ContentView.Field.EditorAction += OnEditorAction;

            ContentView.ResultTable.ItemClick += OnResultClick;
        }

        protected override void OnPause()
        {
            base.OnPause();

            ContentView.Field.TextChanged -= OnTextChange;
            ContentView.Field.EditorAction -= OnEditorAction;

            ContentView.ResultTable.ItemClick -= OnResultClick;
        }

        protected override void SetOnlineMode()
        {
            GeocodingClient.SetOnlineMode();
        }

        protected override void SetOfflineMode()
        {
            GeocodingClient.SetOfflineMode();
        }

        void OnResultClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            OnEditingEnded(false);
            GeocodingResult result = ContentView.Adapter.Items[e.Position];
            ShowResult(result);
        }

		void OnTextChange(object sender, TextChangedEventArgs e)
		{
            string text = e.Text.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            ContentView.ShowTable();
            text = ContentView.Field.Text;

            GeocodingClient.MakeRequest(text, delegate {
                RunOnUiThread(delegate
                {
                    ContentView.UpdateList(GeocodingClient.Addresses);    
                });
            });
		}

		void OnEditorAction(object sender, TextView.EditorActionEventArgs e)
		{
            if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
            {
                OnEditingEnded(true);
            }
        }

        void OnEditingEnded(bool geocode)
        {
            ContentView.CloseKeyboard();
            ContentView.HideTable();

            if (geocode)
            {
                string text = ContentView.Field.Text;

                GeocodingClient.MakeRequest(text, delegate
                {
                    if (GeocodingClient.HasAddress)
                    {
                        GeocodingResult result = GeocodingClient.Addresses[0];
                        ShowResult(result);
                    }

                });
            }

            ContentView.ClearInput();
        }

        void ShowResult(GeocodingResult result)
        {
			string title = "";
            string description = result.GetPrettyAddress();
            bool goToPosition = true;

            ContentView.GeocodingSource.ShowResult(ContentView.MapView, result, title, description, goToPosition);
		}
	}
}
