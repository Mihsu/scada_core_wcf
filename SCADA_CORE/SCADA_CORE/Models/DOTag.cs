using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
	[DataContract]
	public class DOTag : TagInfo
	{
		[DataMember]
		public double InitValue { get; set; }

		public DOTag(string id, string description, string ioAddress, double inVal)
			: base(id, description,"", ioAddress)
		{
			this.InitValue = inVal;
		}
		public DOTag() { }

		public DOTag(string id, string description, string driver, string ioAddress) : base(id, description, driver, ioAddress)
        {
        }

        public override double GetValue()
        {
            throw new NotImplementedException();
        }
    }
}