using System;
namespace CartoMap.iOS
{
	public class StoresVisController : BaseVisController
	{
		public override string Name { get { return "Predicted Store Location"; } }

		public override string Description { get { return "Store location prediction analysis from CARTO.com webpage "; } }

		protected override string Url
		{
			get
			{
				return "https://maps-for-all.cartodb.com/api/v2/viz/78b33d4a-3dd6-11e6-8632-0ea31932ec1d/viz.json";
			}
		}
	}
}

