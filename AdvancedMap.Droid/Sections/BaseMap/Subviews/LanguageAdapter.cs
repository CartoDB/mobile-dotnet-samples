using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Shared.Model;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class LanguageAdapter : ArrayAdapter<Language>
    {
        public List<Language> Languages = new List<Language>();

        public int Width { get; set; }

        public override int Count
        {
            get { return Languages.Count; }
        }

        public LanguageAdapter(Context context, int resource) : base(context, resource)
        {
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            LanguageCell cell;

			Language item = Languages[position];

            if (convertView == null)
            {
                cell = new LanguageCell(Context);

                var height = (int)(40 * Context.Resources.DisplayMetrics.Density);
                cell.LayoutParameters = new AbsListView.LayoutParams(Width, height);
                cell.SetInternalFrame(0, 0, Width, height);
            } 
            else
            {
                cell = convertView as LanguageCell;
            }

            cell.Update(item);
            cell.LayoutSubviews();

            return cell;
        }
    }
}
