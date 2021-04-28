using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;

namespace CovidData.Web.Data
{
    public class CovidDataService
    {
        private readonly CosmosClient cosmosClient;
        public CovidDataService()
        {
            var client = new SecretClient(new Uri("https://swiss-covid-app-keyvault.vault.azure.net/"), new DefaultAzureCredential());

            var secret = client.GetSecret("cosmosDBConnectionString");
            this.cosmosClient = new CosmosClient(secret.Value.Value);
        }

        public async Task<IList<CovidData>> GetCovidData()
        {
            Container covidData = this.cosmosClient.GetContainer("covid-swissData-db", "covid-data");
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<CovidData> feedIterator = covidData.GetItemQueryIterator<CovidData>(queryDefinition);

            List<CovidData> covidDatas= new List<CovidData>();

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<CovidData> roots = await feedIterator.ReadNextAsync();

                foreach (CovidData root in roots)
                {
                    covidDatas.Add(root);
                }
            }

            return covidDatas;
        }
    }
}
