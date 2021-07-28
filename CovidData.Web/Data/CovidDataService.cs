using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using CovidData.Entities;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CovidData.Web.Data
{
    public class CovidDataService
    {
        private readonly CosmosClient cosmosClient;
        public CovidDataService(IConfiguration configuration)
        {
            SecretClient client = new(new Uri(configuration["KeyVaultUri"]), new DefaultAzureCredential());

            var secret = client.GetSecret("cosmosDBConnectionString");
            this.cosmosClient = new CosmosClient(secret.Value.Value);
        }

        public async Task<IList<CovidInfection>> GetCovidData()
        {
            Container covidData = this.cosmosClient.GetContainer("covid-swissData-db", "covid-data");
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<CovidInfection> feedIterator = covidData.GetItemQueryIterator<CovidInfection>(queryDefinition);

            List<CovidInfection> covidDatas= new List<CovidInfection>();

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<CovidInfection> roots = await feedIterator.ReadNextAsync();

                foreach (CovidInfection root in roots)
                {
                    covidDatas.Add(root);
                }
            }

            return covidDatas;
        }
    }
}
