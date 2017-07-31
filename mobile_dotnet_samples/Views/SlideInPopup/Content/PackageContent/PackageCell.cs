
using System;
using UIKit;

namespace Shared.iOS
{
    public class PackageCell : UITableViewCell
    {
        public Package Package { get; private set; }

        public PackageCell()
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public void Update(Package package)
        {
            
        }
    }
}
