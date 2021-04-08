using SCADA_CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA_CORE.DatabaseManagerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDatabaseManagerService" in both code and config file together.
    [ServiceContract]
    public interface IDatabaseManagerService
    {
        [OperationContract]
        bool Register(string username, string password);

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        string AddAnalogOutputTag(string tagId, string description, string ioAddress,int initValue, int lowLimit, int highLimit);

        [OperationContract]
        string AddAnalogInputTag(string tagId, string description, string driver, string ioAddress, int initValue, bool onScan, int lowLimit, int highLimit, string units);


        [OperationContract]
        string AddDigitalOutputTag(string tagId, string description, string ioAddress, int initValue);
        [OperationContract]
        string AddDigitalInputTag(string tagId, string description, string driver,  string ioAddress, int ScanTime, bool onScan);

        [OperationContract]
        string RemoveTag(string tagId);
        [OperationContract]
        string SwitchScanMode(string tagId);

        [OperationContract]
        void LoadXml();

        [OperationContract]
        Dictionary<string, double> GetOutputValues();


        [OperationContract]
        string ChangeOutputValue(string tagId, double newValue);

        [OperationContract]
        string AddAlarmForAnalog(string tagId, string type, int priority);

        [OperationContract]
        string RemoveAlarm(string tagId);
    }
}
