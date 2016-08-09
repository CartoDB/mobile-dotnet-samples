
using System;

namespace CartoMobileSample
{
	public class OfflineVectorMapController : VectorMapBaseController
	{
		public override string Name { get { return "Offline Vector Map"; } }

		public override string Description
		{
			get
			{
				return "A sample that uses bundled asset for offline base map. " +
						 "As MBTilesDataSource can be used only with files residing in file system, " +
						 "the assets needs to be copied first to the SDCard.";
			}
		}
	}
}

