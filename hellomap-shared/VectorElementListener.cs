using Carto.Ui;
using Carto.Layers;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Styles;
using Carto.Utils;

namespace CartoMobileSample
{
	public class VectorElementListener : VectorElementEventListener
	{
		LocalVectorDataSource _dataSource;

		public VectorElementListener(LocalVectorDataSource dataSource)
		{
			_dataSource = dataSource;
		}

		public override bool OnVectorElementClicked(VectorElementClickInfo clickInfo)
		{
			// A note about iOS: DISABLE 'Optimize PNG files for iOS' option in iOS build settings,
			// otherwise icons can not be loaded using AssetUtils/Bitmap constructor as Xamarin converts
			// PNGs to unsupported custom format.
			var bitmap = BitmapUtils.LoadBitmapFromAssets("Icon.png");

			var styleBuilder = new MarkerStyleBuilder();
			styleBuilder.Size = 20;
			styleBuilder.Bitmap = bitmap;
			styleBuilder.Color = new Carto.Graphics.Color(200, 0, 200, 200);
			var marker = new Marker(clickInfo.ClickPos, styleBuilder.BuildStyle());
			_dataSource.Add(marker);

			return true;
		}
	}
}

