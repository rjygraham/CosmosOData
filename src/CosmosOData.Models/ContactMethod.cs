using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CosmosOData.Models
{
	[DataContract]
    public class ContactMethod
    {
		[DataMember]
		public ContactMethodType Type { get; set; }

		[DataMember]
		public PhoneType? PhoneType { get; set; }

		[DataMember]
		public string Value { get; set; }
    }
}
