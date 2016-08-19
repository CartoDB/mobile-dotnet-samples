
using System;
using Carto.PackageManager;

namespace Shared
{
	/**
	* Full package info
	*/
	public class Package
	{
		public string Name { get; private set; }
		public string Id { get; private set; }

		public PackageInfo Info { get; private set; }
		public PackageStatus Status { get; private set; }

		public bool IsSmallerThan1MB { get { return Info.Size.ToLong() < 1024 * 1024; } }

		public Package(string name, PackageInfo info, PackageStatus status)
		{
			this.Name = name;
			this.Id = (info != null ? info.PackageId : null);
			this.Info = info;
			this.Status = status;
		}

		public void UpdateStatus(PackageStatus status)
		{
			Status = status;
		}
	}

}

