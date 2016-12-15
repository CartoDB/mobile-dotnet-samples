using System;
using System.Threading;
using Android.App;
using Android.Content;

namespace Shared.Droid
{
	public class Memory
	{
		static Context Context;

		static Timer timer;

		public static void Log(Context context)
		{
			Context = context;

			ActivityManager manager = (ActivityManager)context.GetSystemService(Context.ActivityService);
			ActivityManager.MemoryInfo info = new ActivityManager.MemoryInfo();
			manager.GetMemoryInfo(info);

			Console.WriteLine("Memory log (" + DateTime.Now.ToString("T") + ")");
			Console.WriteLine("Available: " + info.AvailMem);
			Console.WriteLine("Low: " + info.LowMemory);
			Console.WriteLine("Threshold: " + info.Threshold);
			Console.WriteLine("---------------------------------");

			if (timer == null)
			{
				timer = new Timer(new TimerCallback(OnTimerCallback), null, 1000, 1000);
			}
		}

		static void OnTimerCallback(object state)
		{
			Log(Context);
		}
	}
}
