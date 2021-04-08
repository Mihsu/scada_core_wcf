using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA_CORE.Models
{
    [DataContract]
    public class User
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public User()
        {
        }
    }
}