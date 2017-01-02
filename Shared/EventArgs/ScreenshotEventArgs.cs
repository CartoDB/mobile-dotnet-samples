
using System;

namespace Shared
{
	public class ScreenshotEventArgs : EventArgs
	{
		public Carto.Graphics.Bitmap Bitmap { get; set; }

		public string Path { get; set; }

		public string Message { get; set; }

		public bool IsOK { get { return string.IsNullOrWhiteSpace(Message); } }
	}
}

