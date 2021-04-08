using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
	[DataContract]
	public class AOTag : TagInfo
	{
		[DataMember]
        public double InitValue { get; set; }
		[DataMember]
		public double LowLimit { get; set; }
		[DataMember]
		public double HighLimit { get; set; }
		[DataMember]
		public string Units { get; set; }

		public AOTag(string id, string description, string ioAddress, int init, int low, int high)
			: base(id, description, "", ioAddress)
		{
			this.InitValue = init;
			this.LowLimit = low;
			this.HighLimit = high;
		}
		public AOTag() { }

        public override double GetValue()
        {
            throw new NotImplementedException();
        }
    }
}