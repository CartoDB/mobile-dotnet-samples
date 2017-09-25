using System;
using System.Collections.Generic;
using AdvancedMap.Droid.Sections.BaseMap.Views;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class StylePopupContentSection : BaseView
    {
        public TextView Header { get; private set; }

        BaseView separator;

        public readonly List<StylePopupContentSectionItem> List = new List<StylePopupContentSectionItem>();
        public string Source;

        int rowHeight;

        public int CalculatedHeight
        {
            get
            {
                if (List.Count > 6)
                {
                    return 3 * rowHeight;
                }

                if (List.Count > 3) {
                    return 2 * rowHeight;
                }

                return rowHeight;
            }
        }

        public StylePopupContentSection(Context context) : base(context)
        {
            Header = new TextView(context);
            Header.TextSize = 14.0f;
            Header.Typeface = Typeface.DefaultBold;
            AddView(Header);

            separator = new BaseView(context);
            separator.SetBackgroundColor(Colors.NearWhite);
            AddView(separator);

            rowHeight = (int)(110 * Density);
        }

        public void AddItem(string text, int resource)
        {
            var item = new StylePopupContentSectionItem(Context, text, resource);
            List.Add(item);
            AddView(item);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            int headerHeight = (int)(25 * Density);
            int padding = (int)(5 * Density);

            int x = padding;
            int y = -padding;
            int w = Frame.W - 2 * padding;
            int h = (int)(1 * Density);

            separator.SetFrame(x, y, w, h);

            y = 0;
            w = Frame.W - 3 * padding;
            h = headerHeight;

            Header.SetFrame(x, y, w, h);

            y = headerHeight;
            h = rowHeight - 2 * padding;
            w = (Frame.W - 4 * padding) / 3;

            foreach (var item in List)
            {
                item.SetFrame(x, y, w, h);

                if (x == Frame.W)
                {
                    x = padding;
                    y += rowHeight;
                }
            }
        }

        public StylePopupContentSectionItem FindClickedItem(int x, int y)
        {
            foreach (var item in List)
            {
                x -= Frame.X;
                y -= Frame.Y;

                if (item.HitRect.Contains(x, y))
                {
                    return item;
                }
            }

            return null;
        }

    }
}
