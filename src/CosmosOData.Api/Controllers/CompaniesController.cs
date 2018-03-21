using CosmosOData.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.OData.Sql;
using Microsoft.Azure.Documents.OData.Sql.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CosmosOData.Api.Controllers
{
	public class CompaniesController : ODataController
	{
		private static Uri _collectionLink = UriFactory.CreateDocumentCollectionUri("playground", "cache");

		private DocumentClient _client;
		private ODataToSqlTranslator _translator;

		public CompaniesController(DocumentClient client, ODataToSqlTranslator translator)
		{
			_client = client;
			_translator = translator;
		}

		public IQueryable Get(ODataQueryOptions<Company> options)
		{
			var telemetryClient = new TelemetryClient();

			try
			{
				var sql = _translator.Translate(options, TranslateOptions.ALL).EnforceSingleResult();

				Console.WriteLine($"Executing query: {sql}");

				
				telemetryClient.TrackTrace("Query", new Dictionary<string, string> { { "SQL", sql } });

				var feedOptions = new FeedOptions
				{
					EnableCrossPartitionQuery = true,
					EnableScanInQuery = sql.Contains("STARTSWITH")
				};

				return _client.CreateDocumentQuery(_collectionLink, sql, feedOptions);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Encountered exception executing query: {ex}");
				telemetryClient.TrackException(ex);

				return null;
			}
		}

	}
}
