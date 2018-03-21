# CosmosDB + OData Proof of Concept

This repo is a simple POC of using CosmosDB as the backing store for an AspNetCore OData endpoint. The translation layer was copied from [https://github.com/aboo/azure-documentdb-odata-sql](https://github.com/aboo/azure-documentdb-odata-sql) since that NuGet package is not currently netstandard compatible.

WARNING: This project does not currently support all queries translated from OData to CosmosDB by the project above. There is a minor amount of work required to support all queries.

## Projects
- CosmosOData.Api: The AspNetCore API hosting the OData at the following URI: http://host/odata/Companies
- CosmosOData.DataGenerator: Sample application that uses the Bogus faking library to generate fake data to load into CosmosDB
- CosmosOData.Models: Shared library that contains classes representing the data model for this POC

## Usage
1. Run the CosmosOData.DataGenerator project to create a folder of .json documents containing sample data. The files will be generated in a folder called Output created inside the working folder of the executable.
1. Leverage the [Azure Cosmos DB: Data migration tool](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data) to import the folder of .json documents into CosmosDB
1. Add your CosmosDB Service Endpoint and Auth Key to the CosmosDb section of the appsettings.config in CosmosOData.Api
1. Optional: Add an Application Insights Instrumentation Key to the ApplicationInsights section of the appsettings.config to collect telemetry
1. Run the CosmosOData.Api project and use your favorite web reqeust tool to send requests to the listening endpoint. For example:
    - http://localhost:5000/odata/Companies?$filter=id eq '566'
    - http://localhost:5000/odata/Companies?$filter=startswith(legalName, 'Schneider')
    - http://localhost:5000/odata/Companies?$filter=primaryAddress/line1 eq '3768 Brekke Branch'&$select=id,otherAddresses
1. This application is capable of running from inside a container and you will find a sample Dockerfile in the src folder.

## Acknowledgements
As stated previously, this POC uses code from [https://github.com/aboo/azure-documentdb-odata-sql](https://github.com/aboo/azure-documentdb-odata-sql). If this POC sparks any ideas or helps, I encourage you to visit that repo and submit pull requests to enhance the functionality. Also, if you're using the full framework, you can grab the [NuGet package](https://www.nuget.org/packages/Lambda.Azure.CosmosDb.OData.Sql/).

Finally, if you haven't checked out [Bogus](https://github.com/bchavez/Bogus) for faking your data - I strongly encourage you to do so. It's available as a [NuGet package](https://www.nuget.org/packages/Bogus/) and makes faking ridiculously easy.