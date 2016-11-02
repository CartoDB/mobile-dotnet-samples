using System;
using Android.App;
using Android.Text;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Custom Popup", Description = "Creating and using custom popups")]
	public class CustomPopupActivity : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

			// Initialize a local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(source);

			// Add the previous vector layer to the map
			MapView.Layers.Add(layer);

			// Create marker style
			Android.Graphics.Bitmap androidMarkerBitmap = Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.marker);
			Carto.Graphics.Bitmap markerBitmap = Carto.Utils.BitmapUtils.CreateBitmapFromAndroidBitmap(androidMarkerBitmap);

			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();
			markerStyleBuilder.Bitmap = markerBitmap;
			markerStyleBuilder.Size = 30;
			MarkerStyle markerStyle = markerStyleBuilder.BuildStyle();

			// Add marker
			MapPos berlin = MapView.Options.BaseProjection.FromWgs84(new MapPos(13.38933, 52.51704));
			Marker marker = new Marker(berlin, markerStyle);
			source.Add(marker);

			// Add popup
			PopupStyleBuilder builder = new PopupStyleBuilder();
			builder.SetAttachAnchorPoint(0.5f, 0);
			PopupStyle popupStyle = builder.BuildStyle();

			BasicCustomPopupHandler popupHandler = new BasicCustomPopupHandler("custom popup");

			CustomPopup popup = new CustomPopup(marker, popupStyle, popupHandler);
			popup.SetAnchorPoint(-1, 0);
			source.Add(popup);

			// Animate map to the marker
			MapView.SetFocusPos(berlin, 1);
			MapView.SetZoom(12, 1);
		}
	}

	/************************
	 * Custom Popup Handler *
	 ************************/

	public class BasicCustomPopupHandler : CustomPopupHandler
	{
		const int ScreenPadding = 10;
		const int PopupPadding = 10;
		const int FontSize = 15;
		const int StrokeWidth = 2;
		const int TriangleSize = 10;

		static Android.Graphics.Color TextColor = Android.Graphics.Color.Black;
		static Android.Graphics.Color StrokeColor = Android.Graphics.Color.Black;
		static Android.Graphics.Color BackgroundColor = Android.Graphics.Color.White;

		string text;

		public BasicCustomPopupHandler(string text)
		{
			this.text = text;	
		}

		public override Carto.Graphics.Bitmap OnDrawPopup(PopupDrawInfo popupDrawInfo)
		{
			PopupStyle style = popupDrawInfo.Popup.Style;

			// Calculate scaled dimensions
			float DPToPX = popupDrawInfo.DPToPX;
			float PXTODP = 1 / DPToPX;

			if (style.ScaleWithDPI)
			{
				DPToPX = 1;
			}
			else
			{
				PXTODP = 1;	
			}

			float screenWidth = popupDrawInfo.ScreenBounds.GetWidth() * PXTODP;
			float screenHeight = popupDrawInfo.ScreenBounds.GetHeight() * PXTODP;

			// Update sizes based on scale (uses extension method, cf. Shared/Extensions
			int fontSize = FontSize.Update(DPToPX);

			int triangleWidth = TriangleSize.Update(DPToPX);
			int triangleHeight = TriangleSize.Update(DPToPX);

			int strokeWidth = StrokeWidth.Update(DPToPX);
			int screenPadding = ScreenPadding.Update(DPToPX);

			// Set font
			var font = Android.Graphics.Typeface.Create("HelveticaNeue-Light", Android.Graphics.TypefaceStyle.Normal);

			// Calculate the maximum popup size, adjust with dpi
			int maxPopupWidth = (int)(Math.Min(screenWidth, screenHeight));

			float halfStrokeWidth = strokeWidth * 0.5f;
			int maxTextWidth = maxPopupWidth - (2 * screenPadding + strokeWidth);

			// Measure text
			TextPaint textPaint = new TextPaint { Color = TextColor, TextSize = fontSize };
			textPaint.SetTypeface(font);

			var textLayout = new StaticLayout(text, textPaint, maxTextWidth, Layout.Alignment.AlignNormal, 1, 0, false);

			int textX = (int)Math.Min(textPaint.MeasureText(text), textLayout.Width);
			int textY = textLayout.Height;

			int popupWidth = textX + (2 * PopupPadding + strokeWidth + triangleWidth);
			int popupHeight = textY + (2 * PopupPadding + strokeWidth);

			var bitmap = Android.Graphics.Bitmap.CreateBitmap(popupWidth, popupHeight, Android.Graphics.Bitmap.Config.Argb8888);
			var canvas = new Android.Graphics.Canvas(bitmap);

			var trianglePath = new Android.Graphics.Path();
			trianglePath.MoveTo(triangleWidth, 0);
			trianglePath.LineTo(halfStrokeWidth, triangleHeight * 0.5f);
			trianglePath.LineTo(triangleWidth, triangleHeight);
			trianglePath.Close();

			int triangleOffsetX = 0;
			int triangleOffsetY = (popupHeight - triangleHeight) / 2;

			// Create paint object
			var paint = new Android.Graphics.Paint();
			paint.AntiAlias = true;
			paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
			paint.StrokeWidth = strokeWidth;
			paint.Color = StrokeColor;

			// Stroke background
			var background = new Android.Graphics.RectF();
			background.Left = triangleWidth;
			background.Top = halfStrokeWidth;
			background.Right = popupWidth - strokeWidth;
			background.Bottom = popupHeight - strokeWidth;
			canvas.DrawRect(background, paint);

			// Stroke triangle
			canvas.Save();
			canvas.Translate(triangleOffsetX, triangleOffsetY);
			canvas.DrawPath(trianglePath, paint);
			canvas.Restore();

			// Fill background
			paint.SetStyle(Android.Graphics.Paint.Style.Fill);
			paint.Color = BackgroundColor;
			canvas.DrawRect(background, paint);

			// Fill triangle
			canvas.Save();
			canvas.Translate(triangleOffsetX, triangleOffsetY);
			canvas.DrawPath(trianglePath, paint);
			canvas.Restore();

			if (textLayout != null)
			{
				// Draw text
				canvas.Save();
				canvas.Translate(halfStrokeWidth + triangleWidth + PopupPadding, halfStrokeWidth + PopupPadding);
				textLayout.Draw(canvas);
				canvas.Restore();
			}

			return BitmapUtils.CreateBitmapFromAndroidBitmap(bitmap);
		}

		public override bool OnPopupClicked(PopupClickInfo popupClickInfo)
		{
			return base.OnPopupClicked(popupClickInfo);
		}
	}
}

