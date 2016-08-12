
using System;
using UIKit;

namespace CartoMobileSample
{
	public class PackageManagerButton : UIButton
	{
		public string PackageId { get; set; }

		public string PackageName { get; set; }

		public int PriorityIndex { get; set; }

		public PackageManagerButtonType Type { get; set; }
	}
}

