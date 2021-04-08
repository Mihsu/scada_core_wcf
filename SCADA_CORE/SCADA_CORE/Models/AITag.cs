using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{

	public class AITag : TagInfo
	{
		public double LowLimit { get; set; }
		[DataMember]
		public double HighLimit { get; set; }
		[DataMember]
		public string Units { get; set; }
		public AITag(string id, string description,
						string ioAddress, bool scan, double ll, double hl)
			: base(id, description, "", ioAddress)
		{
			this.OnScan = scan;
			this.LowLimit = ll;
			this.HighLimit = hl;
		}

		public override double GetValue()
		{
			double value = SimulationDriver.ReturnValue(IOAddress);

			if (value > HighLimit) value = HighLimit;
			else if (value < LowLimit) value = LowLimit;

			return value;
		}

        public AITag() { }
	}
}