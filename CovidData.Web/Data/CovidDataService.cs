using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;

namespace CovidData.Web.Data
{
    public class CovidDataService
    {
        private readonly CosmosClient cosmosClient;
        public CovidDataService()
        {
            this.cosmosClient = new CosmosClient("AccountEndpoint=https://covid-swissdata-db.documents.azure.com:443/;AccountKey=xT389nwdQL3ocDl5drKANgtUZKUneorhU9CSewQw43NW6yl0hfKg9s6K1xurVzFkt0BoXYg6ASX08XsNPMF2jQ==;");
            
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
