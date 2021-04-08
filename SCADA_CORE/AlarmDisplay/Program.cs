using System;
using System.Collections.Generic;
using AlarmDisplay.AlarmDisplayServiceReference;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using System.Threading.Tasks;

namespace AlarmDisplay
{
	class Program
	{
		public class CallbackClass : IAlarmDisplayServiceCallback
		{
			public void AlarmActivated(string tagId, int prior, DateTime time)
			{
				for (int i = 0; i < prior; i++)
				{
					Console.WriteLine($"!!! {tagId} Tag alarm activated at {time} !!!");
					Thread.Sleep(1000);
				}
				Console.WriteLine("--------------");
			}
		}

		static void Main(string[] args)
		{
			InstanceContext ic = new InstanceContext(new CallbackClass());
			AlarmDisplayServiceClient proxy = new AlarmDisplayServiceClient(ic);

			proxy.Init();

			Console.ReadKey();
		}
	}
}
