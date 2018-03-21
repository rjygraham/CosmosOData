using Bogus;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace CosmosOData.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			Console.WriteLine("Generating random data...");

			Randomizer.Seed = new Random(86309);
			var companies = GetCompanyFaker(GetAddressFaker(), GetPersonFaker(GetContactMethodFaker()))
				.Generate(5000);

			var fileId = 0;
			Directory.CreateDirectory("Output");

			foreach (var company in companies)
			{
				File.WriteAllText($"Output\\company-{++fileId}.json", JsonConvert.SerializeObject(company));
				Console.WriteLine($"Saved company: {company.Id}");
			}
	
			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static Faker<Models.Address> GetAddressFaker()
		{
			return new Faker<Models.Address>()
				.StrictMode(true)
				.RuleFor(r => r.Id, Guid.NewGuid)
				.RuleFor(r => r.HasStorefront, f => f.Random.Bool())
				.RuleFor(r => r.Line1, f => f.Address.StreetAddress())
				.RuleFor(r => r.Line2, f => f.Random.Bool() ? f.Address.SecondaryAddress() : "")
				.RuleFor(r => r.City, f => f.Address.City())
				.RuleFor(r => r.StateOrProvince, f => f.Address.StateAbbr())
				.RuleFor(r => r.PostalCode, f => f.Address.ZipCode())
				.RuleFor(r => r.Country, f => "US");
		}

		private static Faker<Models.ContactMethod> GetContactMethodFaker()
		{
			return new Faker<Models.ContactMethod>()
				.StrictMode(true)
				.RuleFor(r => r.Type, f => f.PickRandom<Models.ContactMethodType>())
				.RuleFor(r => r.PhoneType, (f, cm) => cm.Type == Models.ContactMethodType.Phone ? f.PickRandom<Models.PhoneType>() : new Nullable<Models.PhoneType>())
				.RuleFor(r => r.Value, (f, cm) => cm.Type == Models.ContactMethodType.Email ? f.Internet.Email() : f.Phone.PhoneNumber());
		}

		private static Faker<Models.Person> GetPersonFaker(Faker<Models.ContactMethod> contactMethodFaker)
		{
			return new Faker<Models.Person>()
				.StrictMode(true)
				.RuleFor(r => r.Id, Guid.NewGuid)
				.RuleFor(r => r.GivenName, f => f.Name.FirstName())
				.RuleFor(r => r.Surname, f => f.Name.LastName())
				.RuleFor(r => r.DisplayName, (f, p) => $"{p.GivenName} {p.Surname}")
				.RuleFor(r => r.PrimaryContactMethod, f => contactMethodFaker.Generate())
				.RuleFor(r => r.OtherContactMethods, f => contactMethodFaker.Generate(f.Random.Int(3, 5)))
				.RuleFor(r => r.Username, (f, p) => f.Internet.UserName(p.GivenName, p.Surname));
		}

		private static Faker<Models.Company> GetCompanyFaker(Faker<Models.Address> addressFaker, Faker<Models.Person> personFaker)
		{
			var id = 0;
			return new Faker<Models.Company>()
				.StrictMode(true)
				.RuleFor(r => r.Id, f => (++id).ToString())
				.RuleFor(r => r.DisplayName, f => f.Company.CompanyName())
				.RuleFor(r => r.LegalName, f => f.Company.CompanyName())
				.RuleFor(r => r.PrimaryAddress, f => addressFaker.Generate())
				.RuleFor(r => r.OtherAddresses, f => addressFaker.Generate(f.Random.Int(2, 5)))
				.RuleFor(r => r.People, f => personFaker.Generate(f.Random.Int(10, 50)));
		}
	}
}
