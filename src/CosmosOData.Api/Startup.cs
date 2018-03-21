using CosmosOData.Api.Settings;
using CosmosOData.Models;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.OData.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CosmosOData.Api
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

			services.AddOData();

			var cosmosDbSettings = Configuration.GetSection("CosmosDb").Get<CosmosDbSettings>();

			var serviceEndpoint = Environment.GetEnvironmentVariable("ServiceEndpoint") ?? cosmosDbSettings.ServiceEndpoint;
			var authKey = Environment.GetEnvironmentVariable("AuthKey") ?? cosmosDbSettings.AuthKey;

			services.AddSingleton(new DocumentClient(new Uri(serviceEndpoint), authKey,
				new ConnectionPolicy
				{
					ConnectionMode = ConnectionMode.Gateway,
					ConnectionProtocol = Protocol.Tcp
				}));

			services.AddSingleton(new ODataToSqlTranslator(new SQLQueryFormatter()));
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			var builder = new ODataConventionModelBuilder(app.ApplicationServices);
			builder.EntitySet<Company>("Companies");
			builder.EnableLowerCamelCase();
			var model = builder.GetEdmModel();

			app.UseMvc(routeBuilder =>
			{

				routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(1).Count();
				routeBuilder.MapODataServiceRoute(routeName: "OData", routePrefix: "odata", model: model);

				routeBuilder.EnableDependencyInjection();

				routeBuilder.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"); // enable mvc controllers
			});

			app.UseODataBatching();
		}
    }
}
