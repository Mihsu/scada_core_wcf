using System;
using System.ServiceModel;

namespace SCADA_CORE.AlarmDisplayService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlarmDisplayService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select AlarmDisplayService.svc or AlarmDisplayService.svc.cs at the Solution Explorer and start debugging.
	public class AlarmDisplayService : IAlarmDisplayService
	{
		public delegate void AlarmActivated(string tagId, int prior, DateTime time);
		public static event AlarmActivated AlarmActivatedHandler = null;

		public void Init()
		{
			IAlarmDisplayCallback proxy = OperationContext.Current.GetCallbackChannel<IAlarmDisplayCallback>();
			AlarmActivatedHandler = proxy.AlarmActivated;
		}

		public static void Notify(string tagId, int prior, DateTime time)
		{
            AlarmActivatedHandler?.Invoke(tagId, prior, time);
        }

	}
}
