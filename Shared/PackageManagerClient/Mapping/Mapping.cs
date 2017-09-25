using System;
namespace Shared.PackageManagerClient.Mapping
{
    public class Mapping : BasePackageManagerClient 
    {
		public override string Source
		{
			get { return Sources.CartoVector; }
		}

		public const string PackageFolder = "com.carto.mappackages";
		
        public Mapping(string path) : base(path)
        {

		}
	}
}
