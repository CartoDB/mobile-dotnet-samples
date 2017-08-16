
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class PackagePopupContent : BaseView
    {
        public ListView List { get; private set; }
        public PackageAdapter Adapter { get; private set; }

        public PackagePopupContent(Context context, int icon_forward) : base(context)
        {
            Adapter = new PackageAdapter(context, icon_forward);
            List = new ListView(context);
            List.Adapter = Adapter;

            AddView(List);
        }

        public void AddPackages(List<Package> packages)
        {
            Adapter.Packages.Clear();
            Adapter.Packages.AddRange(packages);
            Adapter.NotifyDataSetChanged();
        }

        public void FindAndUpdate(string id, PackageStatus status)
        {
            Find(id)?.Update(status);
        }

        PackageCell Find(string id)
        {
            for (int i = 0; i < List.ChildCount; i++)
            {
                var child = List.GetChildAt(i);

                if (child is PackageCell)
                {
                    var cell = child as PackageCell;
                    if (cell.Package.Id.Equals(id))
                    {
                        return cell;
                    }
                }
            }

            return null;
        }

    }
}
