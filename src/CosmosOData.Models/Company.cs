using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CosmosOData.Models
{
	[DataContract]
    public class Company
    {
		[Key]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public string LegalName { get; set; }

		[DataMember]
		public Address PrimaryAddress { get; set; }

		[DataMember]
		public List<Address> OtherAddresses { get; set; }

		[DataMember]
		public List<Person> People { get; set; }
    }
}
