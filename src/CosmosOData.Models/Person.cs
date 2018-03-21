using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CosmosOData.Models
{
	[DataContract]
    public class Person
	{ 
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string GivenName { get; set; }

		[DataMember]
		public string Surname { get; set; }

		[DataMember]
		public string DisplayName { get => $"{GivenName} {Surname}"; }

		[DataMember]
		public ContactMethod PrimaryContactMethod { get; set; }

		[DataMember]
		public List<ContactMethod> OtherContactMethods { get; set; }

		[DataMember]
		public string Username { get; set; }
    }
}
