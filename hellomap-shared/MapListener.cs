using Carto.Core;
using Carto.Ui;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Styles;
using Carto.Utils;
using Carto.Graphics;

namespace CartoMobileSample
{

	public class MapListener : MapEventListener
	{
		LocalVectorDataSource _dataSource;

		public MapListener(LocalVectorDataSource dataSource)
		{
			_dataSource = dataSource;
		}

		public override void OnMapClicked (MapClickInfo mapClickInfo)
		{
			// Add default marker to the click location
			var styleBuilder = new MarkerStyleBuilder ();
			styleBuilder.Size = 10;
			var marker = new Marker (mapClickInfo.ClickPos, styleBuilder.BuildStyle());
			_dataSource.Add (marker);
		}

		public override void OnMapMoved()
		{
			
		}
	}
}
