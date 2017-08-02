
using System;
using CoreGraphics;
using Foundation;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class GeocodingView : BaseGeocodingView
    {
        UITextField inputField;
        UITableView resultTable;

        UIFont font = UIFont.FromName("HelveticaNeue", 15);

        public GeocodingView()
        {
            inputField = new UITextField();
            inputField.TextColor = UIColor.White;
            inputField.BackgroundColor = Colors.DarkTransparentGray;
            inputField.AutocorrectionType = UITextAutocorrectionType.No;
            inputField.Font = font;
            // Text padding
            inputField.LeftView = new UIView(new CGRect(0, 0, 10, 20));
            inputField.LeftViewMode = UITextFieldViewMode.Always;
            AddSubview(inputField);

            resultTable = new UITableView();
            resultTable.Hidden = true;
            resultTable.AllowsSelection = true;
            resultTable.UserInteractionEnabled = true;
            resultTable.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            AddSubview(resultTable);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = 5.0f;

            nfloat x = padding;
            nfloat y = Device.TrueY0 + padding;
            nfloat w = Frame.Width - 2 * padding;
            nfloat h = 50;

            inputField.Frame = new CGRect(x, y, w, h);

            y += h + 1;
            h = 240;

            resultTable.Frame = new CGRect(x, y, w, h);
        }

        string placeHolder1 = "Download a package to start geocoding";
		string placeholder2 = "Type address...";

		public void SetPlaceholder(bool localPackagesExist)
        {
            if (localPackagesExist)
            {
                SetPlaceholder(placeholder2);
                inputField.UserInteractionEnabled = true;
            }
            else
            {
                SetPlaceholder(placeHolder1);
                inputField.UserInteractionEnabled = false;
            }
        }

        void SetPlaceholder(string text)
        {
            UIStringAttributes attributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(200, 200, 200),
                Font = font
            };

            inputField.AttributedPlaceholder = new NSAttributedString(text, attributes);
        }
    }
}
