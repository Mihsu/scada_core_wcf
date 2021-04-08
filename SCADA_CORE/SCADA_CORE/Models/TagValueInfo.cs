using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
    [DataContract]
    public class TagValueInfo
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string TagId { get; set; }

        [DataMember]
        public double Value { get; set; }

        public TagValueInfo(string TagId, double Value)
        {
            this.TagId = TagId;
            this.Value = Value;
        }
    }
}