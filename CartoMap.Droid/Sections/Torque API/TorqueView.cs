
using System;
using Android.Content;
using Android.Widget;
using Carto.Ui;

namespace CartoMap.Droid
{
	public class TorqueView : RelativeLayout
	{
		public MapView MapView { get; private set; }

		public TorqueHistogram Histogram { get; private set; }

		int HistogramMargin { get { return (int)(5f * Context.Resources.DisplayMetrics.Density); } }

		public TorqueView(Context context) : base(context)
		{
			MapView = new MapView(context);
			AddView(MapView);

			MapView.LayoutParameters = new RelativeLayout.LayoutParams(
				RelativeLayout.LayoutParams.MatchParent, 
				RelativeLayout.LayoutParams.MatchParent
			);

			Histogram = new TorqueHistogram(context);
			AddView(Histogram);
		}

		public new void Dispose()
		{
			MapView.Dispose();
			GC.Collect(0);
			base.Dispose();
		}

		public void InitializeHistogram(int frameCount)
		{
			Histogram.Initialize(frameCount, HistogramMargin);
		}
	}
}
