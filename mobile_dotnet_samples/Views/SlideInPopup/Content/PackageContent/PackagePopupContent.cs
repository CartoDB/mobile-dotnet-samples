﻿
using System.Collections.Generic;
using Carto.PackageManager;
using UIKit;

namespace Shared.iOS
{
    public class PackagePopupContent : UIView
    {
		UITableView table;

        public PackageListSource Source
        {
            get { return table.Source as PackageListSource; }
        }

        public PackagePopupContent()
        {
            table = new UITableView();
            table.RegisterClassForCellReuse(typeof(PackageCell), PackageListSource.Identifier);
            table.Source = new PackageListDataSource();
            AddSubview(table);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            table.Frame = Bounds;
        }

        public void AddPackages(List<Package> packages)
        {
            Source.Packages.Clear();
            Source.Packages.AddRange(packages);
            table.ReloadData();
        }

        public void FindAndUpdate(Package package, float progress)
        {
            Find(package.Id)?.Update(package, progress);    
        }

        public void FindAndUpdate(Package package)
        {
            Find(package.Id)?.Update(package);
        }

        public void FindAndUpdate(string id, PackageStatus status)
        {
            Find(id)?.Update(status);
        }

        PackageCell Find(string id)
        {
            foreach (PackageCell cell in table.VisibleCells)
            {
                if (cell.Package.Id == id)
                {
                    return cell;
                }
            }

            return null;
        }
	}
}
