
using System;
using Carto.Geocoding;
using Foundation;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class GeocodingController : BaseGeocodingController, IUITextFieldDelegate, IUITableViewDelegate, IUITableViewDataSource
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ContentView = new GeocodingView();
            View = ContentView;

            Geocoding.Projection = ContentView.Projection;

			Title = "GEOCODING";

            SetOnlineMode();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            (ContentView as GeocodingView).InputField.Delegate = this;
            (ContentView as GeocodingView).ResultTable.Delegate = this;
            (ContentView as GeocodingView).ResultTable.DataSource = this;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        string InputText
        {
            get { return (ContentView as GeocodingView).InputField.Text; }
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            (ContentView as GeocodingView).FinishEditing();

            string text = (ContentView as GeocodingView).InputField.Text;
            Geocoding.MakeRequest(text, delegate
            {
                InvokeOnMainThread(delegate
                {
					if (Geocoding.HasAddress)
					{
						GeocodingResult result = Geocoding.Addresses[0];
						ShowResult(result);
					}
					else
					{
						Alert("Unable to find any results. What did you just type in?");
					}
				});

            });

            return true;
        }

        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            (ContentView as GeocodingView).ResultTable.Hidden = false;

            var substring = new NSString(InputText).Replace(range, new NSString(replacementString));

            Geocoding.MakeRequest(substring, delegate
            {
                InvokeOnMainThread(delegate
                {
                    (ContentView as GeocodingView).ResultTable.ReloadData();
                });

            });

            return true;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            (ContentView as GeocodingView).FinishEditing();

            GeocodingResult result = Geocoding.Addresses[indexPath.Row];
            ShowResult(result);
        }

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            var cell = tableView.DequeueReusableCell(GeocodingView.Identifier, indexPath);

            var row = indexPath.Row;
            var result = Geocoding.Addresses[row];
            cell.Tag = row;
            cell.TextLabel.Text = result.GetPrettyAddress();
            cell.TextLabel.Font = (ContentView as GeocodingView).font;
            cell.TextLabel.TextColor = UIColor.White;
            cell.BackgroundColor = Colors.LightTransparentGray;
            cell.TextLabel.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);

            return cell;
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
            return Geocoding.Addresses.Count;
		}

        void ShowResult(GeocodingResult result)
        {
            var title = "";
            var description = result.GetPrettyAddress();
            var goToPosition = true;

            ContentView.ObjectSource.ShowResult(ContentView.MapView, result, title, description, goToPosition);
        }

		public override void SetOnlineMode()
		{
			Geocoding.SetOnlineMode();
		}

		public override void SetOfflineMode()
		{
            Geocoding.SetOfflineMode();
		}
	}
}
