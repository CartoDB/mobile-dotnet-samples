
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Shared.iOS;

namespace AdvancedMap.iOS.Sections.BaseMap.Subviews
{
    public class StylePopupContentSection : UIView
    {
        public UILabel Header { get; private set; }

        UIView separator;

        public readonly List<StylePopupContentSectionItem> List = new List<StylePopupContentSectionItem>();
        public string Source;
        readonly int rowHeight;

        public int CalculatedHeight
        {
            get
            {
                if (List.Count > 6)
                {
                    return 3 * rowHeight;
                }

                if (List.Count > 3)
                {
                    return 2 * rowHeight;
                }

                return rowHeight;
            }
        }

        public StylePopupContentSection()
        {
            Header = new UILabel();
            Header.Font = UIFont.FromName("HelveticaNeue-Bold", 14.0f);
            AddSubview(Header);

            separator = new UIView();
            separator.BackgroundColor = Colors.NearWhite;
            AddSubview(separator);

            rowHeight = 110;
        }

        public void AddItem(string text, string resource)
        {
            var item = new StylePopupContentSectionItem(text, resource);
            List.Add(item);
            AddSubview(item);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            int headerHeight = 25;
            int padding = 5;

            nfloat x = padding;
            nfloat y = -padding;
            nfloat w = Frame.Width - 2 * padding;
            nfloat h = 1;

            separator.Frame = new CGRect(x, y, w, h);

            y = 0;
            w = Frame.Width - 3 * padding;
            h = headerHeight;

            Header.Frame = new CGRect(x, y, w, h);

            y = headerHeight;
            h = rowHeight - 2 * padding;
            w = (Frame.Width - 4 * padding) / 3;

            foreach (var item in List)
            {
                item.Frame = new CGRect(x, y, w, h);

                x += w + padding;

                if (x == Frame.Width)
                {
                    x = padding;
                    y += rowHeight;
                }
            }
        }

    }
}
