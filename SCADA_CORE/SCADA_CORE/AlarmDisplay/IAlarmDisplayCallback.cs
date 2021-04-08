using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace SCADA_CORE.AlarmDisplayService
{
    public interface IAlarmDisplayCallback
    {
        [OperationContract(IsOneWay = true)]
        void AlarmActivated(string tagId, int prior, DateTime time);
    }
}