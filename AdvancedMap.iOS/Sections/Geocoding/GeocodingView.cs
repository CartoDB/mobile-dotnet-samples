
using System;
using CoreGraphics;
using Foundation;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class GeocodingView : BaseGeocodingView
    {
		public const string Identifier = "AutocompleteRowId";

		public UITextField InputField { get; private set; }

        public UITableView ResultTable { get; private set; }

        public readonly UIFont font = UIFont.FromName("HelveticaNeue", 15);

        public GeocodingView()
        {
            InputField = new UITextField();
            InputField.TextColor = UIColor.White;
            InputField.BackgroundColor = Colors.DarkTransparentGray;
            InputField.AutocorrectionType = UITextAutocorrectionType.No;
            InputField.Font = font;
            // Text padding
            InputField.LeftView = new UIView(new CGRect(0, 0, 10, 20));
            InputField.LeftViewMode = UITextFieldViewMode.Always;
            AddSubview(InputField);

            ResultTable = new UITableView();
            ResultTable.Hidden = true;
            ResultTable.AllowsSelection = true;
            ResultTable.UserInteractionEnabled = true;
            ResultTable.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            ResultTable.RegisterClassForCellReuse(typeof(UITableViewCell), Identifier);
            AddSubview(ResultTable);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = 5.0f;

            nfloat x = padding;
            nfloat y = Device.TrueY0 + padding;
            nfloat w = Frame.Width - 2 * padding;
            nfloat h = 50;

            InputField.Frame = new CGRect(x, y, w, h);

            y += h + 1;
            h = 240;

            ResultTable.Frame = new CGRect(x, y, w, h);
        }

        string placeHolder1 = "Download a package to start geocoding";
		string placeholder2 = "Type address...";

		public void SetPlaceholder(bool localPackagesExist)
        {
            if (localPackagesExist)
            {
                SetPlaceholder(placeholder2);
                InputField.UserInteractionEnabled = true;
            }
            else
            {
                SetPlaceholder(placeHolder1);
                InputField.UserInteractionEnabled = false;
            }
        }

        void SetPlaceholder(string text)
        {
            UIStringAttributes attributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(200, 200, 200),
                Font = font
            };

            InputField.AttributedPlaceholder = new NSAttributedString(text, attributes);
        }

        public void FinishEditing()
        {
            InputField.ResignFirstResponder();
            ResultTable.Hidden = true;
        }
    }
}
