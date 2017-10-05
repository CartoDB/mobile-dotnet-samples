using System;
namespace CartoMap.iOS
{
	public class TrainVisController : BaseVisController
	{
		public override string Name { get { return "NYC L-train Vis"; } }

		public override string Description { get { return "L-train travel area analysis visualisation"; } }

		protected override string Url
		{
			get
			{
				// NB! This was fixed in a later snapshot, there are some issues with it in our 4.0.0 release version
				return "https://mamataakella.cartodb.com/api/v2/viz/30730478-bbb5-11e5-b75c-0e5db1731f59/viz.json";
			}
		}
	}
}

