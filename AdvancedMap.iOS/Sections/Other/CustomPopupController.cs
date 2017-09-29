
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class CustomPopupController : MapBaseController
	{
		public override string Name { get { return "Custom Popup"; } }

		public override string Description { get { return "Creating and using custom popups"; } }

		Bitmap Bitmap
		{
			get { 
				UIImage image = UIImage.FromFile("icons/marker.png"); 
				return BitmapUtils.CreateBitmapFromUIImage(image);
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Add default base layer
			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);
			VectorLayer layer = new VectorLayer(source);
			MapView.Layers.Add(layer);

			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Bitmap = Bitmap;
			builder.Size = 20;

			// Add marker
			MapPos position = BaseProjection.FromWgs84(new MapPos(24.646469, 59.426939));
			Marker marker = new Marker(position, builder.BuildStyle());
			source.Add(marker);

			// Custom popup
			PopupStyleBuilder popupBuilder = new PopupStyleBuilder();
			popupBuilder.SetAttachAnchorPoint(0.5f, 0);

			// Initialize our custom handler, cf. class below
			MyCustomPopupHandler handler = new MyCustomPopupHandler();

			CustomPopup popup = new CustomPopup(marker, popupBuilder.BuildStyle(), handler);
			popup.SetAnchorPoint(-1.0f, 0.0f);
			source.Add(popup);
		}
	}

	public class MyCustomPopupHandler : CustomPopupHandler
	{
		const int ScreenPadding = 10;
		const int PopupPadding = 10;
		const int FontSize = 15;
		const int StrokeWidth = 2;
		const int TriangleSize = 10;

		static byte[] TextColorBytes = { 0, 0, 0, 255 };
		static byte[] StrokeColorBytes = TextColorBytes;
		static byte[] BackgroundColorBytes = { 255, 255, 255, 255 };

		static UIColor TextColor = TextColorBytes.ToUIColor();
		static UIColor StrokeColor = StrokeColorBytes.ToUIColor();
		static UIColor BackgroundColor = BackgroundColorBytes.ToUIColor();

		public override Bitmap OnDrawPopup(PopupDrawInfo popupDrawInfo)
		{
			string text = "Custom Popup";

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

			UIFont font = UIFont.FromName("HelveticaNeue-Light", fontSize);

			// Calculate the maximum popup size, adjust with dpi
			float maxPopupWidth = Math.Min(screenWidth, screenHeight);

			// Calculate maximum text and description width
			float halfStrokeWidth = strokeWidth * 0.5f;
			float maxTextWidth = maxPopupWidth - (2 * screenPadding + strokeWidth);

			CGSize textSize = text.StringSize(font, new CGSize(maxTextWidth, nfloat.MaxValue));

			// Calculate bitmap size
			nfloat popupWidth = textSize.Width + 2 * PopupPadding + strokeWidth + triangleWidth;
			nfloat popupHeight = textSize.Height + 2 * PopupPadding + strokeWidth;
			CGSize popupSize = new CGSize(popupWidth, popupHeight);

			// Create graphics context;
			UIGraphics.BeginImageContext(popupSize);

			CGContext context = UIGraphics.GetCurrentContext();
			CGRect backgroundStrokeRect = new CGRect(
				triangleWidth, 
				halfStrokeWidth, 
				popupWidth - (strokeWidth + triangleWidth), 
				popupHeight - strokeWidth
			);

			UIBezierPath backgroundPath = UIBezierPath.FromRoundedRect(backgroundStrokeRect, 1);
			backgroundPath.LineWidth = strokeWidth;

			// Prepare triangle path
			CGPath trianglePath = new CGPath();
			trianglePath.MoveToPoint(triangleWidth, 0);
			trianglePath.AddLineToPoint(halfStrokeWidth, triangleHeight * 0.5f);
			trianglePath.AddLineToPoint(triangleWidth, triangleHeight);
			trianglePath.CloseSubpath();

			// Calculate anchor point and triangle position
			nfloat triangleOffsetX = 0;
			nfloat triangleOffsetY = (popupHeight - triangleHeight) / 2;

			// Stroke background
			StrokeColor.SetStroke();
			backgroundPath.Stroke();

			// Stroke triangle
			context.SaveState();
			context.TranslateCTM(triangleOffsetX, triangleOffsetY);
			context.SetLineWidth(strokeWidth);
			context.AddPath(trianglePath);
			context.SetStrokeColor(StrokeColor.CGColor);
			context.StrokePath();

			context.RestoreState();

			// Fill background
			BackgroundColor.SetFill();
			backgroundPath.Fill();

			// Fill triangle
			context.SaveState();
			context.TranslateCTM(triangleOffsetX, triangleOffsetY);
			context.AddPath(trianglePath);
			context.SetFillColor(BackgroundColor.CGColor);
			context.FillPath();

			context.RestoreState();

			// Draw text
			context.SetFillColor(TextColor.CGColor);
			CGRect textRect = new CGRect(
				halfStrokeWidth + PopupPadding + triangleWidth, 
				PopupPadding, 
				textSize.Width, 
				textSize.Height
			);

			text.DrawString(textRect, font, UILineBreakMode.WordWrap);

			// Extract image
			UIImage image = UIGraphics.GetImageFromCurrentImageContext();

			// Clean up
			UIGraphics.EndImageContext();

			return BitmapUtils.CreateBitmapFromUIImage(image);
		}
	}

	public static class ColorExtensions
	{
		public static UIColor ToUIColor(this byte[] bytes)
		{
			return UIColor.FromRGBA(bytes[0], bytes[1], bytes[2], bytes[3]);
		}

		public static Color ToCartoColor(this byte[] bytes)
		{
			return new Color(bytes[0], bytes[1], bytes[2], bytes[3]);
		}
	}
}

