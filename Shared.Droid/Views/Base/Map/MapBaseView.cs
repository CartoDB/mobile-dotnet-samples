
using System;
using System.Collections.Generic;
using Android.Content;
using Carto.Projections;
using Carto.Ui;

namespace Shared.Droid
{
    public class MapBaseView : BaseView
    {
		public MapView MapView { get; private set; }

		public SlideInPopup Popup { get; private set; }

		public Projection Projection
		{
			get { return MapView.Options.BaseProjection; }
		}

		public MapBaseView(Context context, int backIcon, int closeIcon) : base(context)
        {
			Popup = new SlideInPopup(context, backIcon, closeIcon);
			AddView(Popup);

			MapView = new MapView(context);
            AddView(MapView);
        }

		protected int BottomLabelHeight
        {
            get { return (int)(40 * Density); }
        }

		protected int SmallPadding
		{
			get { return (int)(5 * Density); }
		}

		public override void LayoutSubviews()
        {
            int x = 0;
            int y = 0;
            int w = Frame.W;
            int h = Frame.H;

            MapView.SetFrame(x, y, w, h);
            Popup.Frame = new CGRect(x, y, w, h);

			int count = buttons.Count;

            var buttonWidth = (int)(60 * Density);
            var innerPadding = (int)(25 * Density);
			var totalArea = buttonWidth * count + (innerPadding * (count - 1));

			w = buttonWidth;
			h = w;
            y = Frame.H - (BottomLabelHeight + h + SmallPadding);
            x = Frame.W / 2 - totalArea / 2;

            foreach (ActionButton button in buttons)
			{
				button.Frame = new CGRect(x, y, w, h);
				x += w + innerPadding;
			}
        }

        readonly List<ActionButton> buttons = new List<ActionButton>();

        public void AddButton(ActionButton button)
        {
            buttons.Add(button);
            AddView(button);
        }

    }
}
