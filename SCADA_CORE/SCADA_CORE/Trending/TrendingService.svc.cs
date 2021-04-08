using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA_CORE.TrendingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingService.svc or TrendingService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingService : ITrendingService
    {
        public delegate void TagValueChanged(string id, double value);
        public static event TagValueChanged TagValueChangedHandler;

        public void Init()
        {
            ITrendingServiceCallback serviceCallbackProxy = OperationContext.Current.GetCallbackChannel<ITrendingServiceCallback>();
            TagValueChangedHandler = serviceCallbackProxy.TagValueChangedMethod;
        }

        public static void FireEvent(string tagId, double value)
        {
            TagValueChangedHandler?.Invoke(tagId, value);
        }
    }
}
