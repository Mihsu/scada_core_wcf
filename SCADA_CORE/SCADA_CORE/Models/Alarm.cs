using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
	[DataContract]
	// [KnownType(typeof(AnalogAlarm))]
	// [KnownType(typeof(DigitalAlarm))]
	public class Alarm
    {
		[Key]
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string TagId { get; set; }

		[DataMember]
		public int Priority { get; set; }

		[DataMember]
		public string Type { get; set; }
		 
		[DataMember]
		public DateTime Time { get; set; }

		[DataMember]
		public bool Activated { get; set; }

		public Alarm(string type, int prior, DateTime time, string tagId, bool activated)
		{
			Type = type;
			Priority = prior;
			Time = time;
			TagId = tagId;
			Activated = activated;
		}
		public Alarm() { }
	}
}