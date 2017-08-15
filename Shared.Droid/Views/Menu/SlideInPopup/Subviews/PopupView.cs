
using System;
using Android.Content;

namespace Shared.Droid
{
	public class PopupView : BaseView
	{
        public PopupHeader Header { get; private set; }

		public PopupView(Context context, int backIcon, int closeIcon) : base(context)
		{
            Header = new PopupHeader(context, backIcon, closeIcon);
            AddView(Header);
		}

        public override void LayoutSubviews()
        {
            Header.Frame = new CGRect(0, 0, Frame.W, Header.TotalHeight);
        }
	}
}
