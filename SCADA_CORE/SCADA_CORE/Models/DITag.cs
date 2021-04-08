using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
	public class DITag : TagInfo
	{	
		public DITag(string id, string description, string driver, string ioAddress, bool scan)
			: base(id, description, driver, ioAddress)
		{
			this.OnScan = scan;
		}
		public DITag() { }
		public override double GetValue()
		{
			if (Driver == "Sim driver")
			{
				return SimulationDriver.ReturnValue(IOAddress) ;
			}
			else
			{
				return TagProcessing.AddressValues[IOAddress];
			}
		}
	}
}