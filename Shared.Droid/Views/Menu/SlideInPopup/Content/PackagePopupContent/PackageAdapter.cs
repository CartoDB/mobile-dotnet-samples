
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
    public class PackageAdapter : ArrayAdapter<Package>
    {
        public List<Package> Packages { get; private set; } = new List<Package>();

        public ListView List { get; set; }
        public int Width { get; set; }

        int icon_forward = -1;

        public PackageAdapter(Context context, int icon_forward) : base(context, -1)
        {
            this.icon_forward = icon_forward;
        }

        public override int Count { get { return Packages.Count; } }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            PackageCell cell = null;
            Package package = Packages[position];

            if (convertView == null)
            {
                cell = new PackageCell(Context, icon_forward);
                int height = (int)(45 * Context.Resources.DisplayMetrics.Density);
                cell.LayoutParameters = new AbsListView.LayoutParams(Width, height);
                cell.SetInternalFrame(0, 0, Width, height);
            }
            else
            {
                cell = (PackageCell)convertView;
            }

            cell.Update(package);
            cell.LayoutSubviews();

            return cell;
        }
    }
}
