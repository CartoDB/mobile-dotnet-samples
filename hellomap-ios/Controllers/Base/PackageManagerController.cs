
using System;

namespace CartoMobileSample
{
	public class PackageManagerController : MapBaseController
	{
		public override string Name { get { return "Package Manager"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use offline package manager of the Carto Mobile SDK. " +
						 "The sample downloads the latest package list from Carto online service, " +
						 "displays this list and allows user to manage offline packages";
			}
		}

	}
}