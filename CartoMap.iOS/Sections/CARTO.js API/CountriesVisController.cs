using System;
namespace CartoMap.iOS
{
	public class CountriesVisController : BaseVisController
	{
		public override string Name { get { return "Countries Vis"; } }

		public override string Description { get { return "Vis displaying countries in different colors"; } }

		protected override string Url
		{
			get
			{
				return "http://documentation.cartodb.com/api/v2/viz/2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json";
			}
		}
	}
}

