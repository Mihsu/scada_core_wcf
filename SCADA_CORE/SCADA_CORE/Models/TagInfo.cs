using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{

	public abstract class TagInfo
    {
		public string TagId { get; set; }
		public string Description { get; set; }
		public string Driver { get; set; }
		public string IOAddress { get; set; }
		public int ScanTime { get; set; }
		public bool OnScan { get; set; }
		public abstract double GetValue();
		public TagInfo(string tagId, string description, string driver, string ioAddress)
		{
			this.TagId = tagId;
			this.Description = description;
			this.Driver = driver;
			this.IOAddress = ioAddress;
		}
		public TagInfo() { }
	}
}