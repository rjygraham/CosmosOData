using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CosmosOData.Models
{
	[DataContract]
	public class Address
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public bool HasStorefront { get; set; }

		[DataMember]
		public string Line1 { get; set; }

		[DataMember]
		public string Line2 { get; set; }

		[DataMember]
		public string City { get; set; }

		[DataMember]
		public string StateOrProvince { get; set; }

		[DataMember]
		public string PostalCode { get; set; }

		[DataMember]
		public string Country { get; set; }
	}
}
