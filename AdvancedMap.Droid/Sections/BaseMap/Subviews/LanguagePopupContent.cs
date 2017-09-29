using System;
using Android.Content;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class LanguagePopupContent : BaseView
    {
        public ListView List { get; private set; }
        public LanguageAdapter Adapter { get; private set; }

        public LanguagePopupContent(Context context) : base(context)
        {
            List = new ListView(context);
            AddView(List);

            Adapter = new LanguageAdapter(context, -1);
            List.Adapter = Adapter;
        }

        public override void LayoutSubviews()
        {
            Adapter.Width = Frame.W;
            List.SetFrame(0, 0, Frame.W, Frame.H);
        }
    }
}
