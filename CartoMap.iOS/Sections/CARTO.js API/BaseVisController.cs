
using System;
using System.Collections.Generic;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class BaseVisController : MapBaseController
	{
		public override string Name { get { return null; } }

		public override string Description { get { return null; } }

		protected virtual string Url { get { return null; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView.UpdateVisWithGridEvent(Url, OnError);

			//MapView.InitializeVectorTileListener(new VectorLayer(new Loca);
		}

		void OnError(string message)
		{
			Alert(message);
		}
	}
}

