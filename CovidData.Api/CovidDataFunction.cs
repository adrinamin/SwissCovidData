using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CovidData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CovidData.Api
{
    public static class CovidDataFunction
    {
        [FunctionName("GetCovidData")]
        public static async Task<IActionResult> GetCovidData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "covid")] HttpRequest req,
            [CosmosDB(
                databaseName:"covid-swissData-db", 
                collectionName:"covid-data", 
                ConnectionStringSetting = "CosmosDBConnection", 
                SqlQuery = "SELECT * FROM c")] IEnumerable<CovidInfection> covidInfections,
            ILogger logger)
        {
            logger.LogInformation("Get all covid data.");

            foreach (var covidInfection in covidInfections)
            {
                logger.LogInformation($"covid data: {covidInfection.id}, {covidInfection.location}, {covidInfection.date}");
            }
            
            return new OkObjectResult(covidInfections);
        }

        [FunctionName("GetCovidDataById")]
        public static async Task<IActionResult> GetCovidDataById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "covid/{id}")] HttpRequest req,
            [CosmosDB(                
                databaseName:"covid-swissData-db",
                collectionName:"covid-data",
                ConnectionStringSetting = "CosmosDBConnection", 
                SqlQuery = "select * from c where c.id = {id}")] IEnumerable<CovidInfection> covidInfections,
            ILogger logger)
        {
            var covidInfection = covidInfections.First();
            logger.LogInformation($"Get covid data by id: {covidInfection.id}, {covidInfection.location}, {covidInfection.date}");
            return new OkObjectResult(covidInfections);
        }

        [FunctionName("CreateCovidDataEntry")]
        public static void CreateCovidDataEntry(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "covid")] HttpRequest request,
            [CosmosDB(
                databaseName: "covid-swissData-db",
                collectionName: "covid-data",
                ConnectionStringSetting = "CosmosDBConnection")] out dynamic document,
            ILogger logger)
        {
            var requestBody = request.Body;
            var streamReader = new StreamReader(requestBody);
            var covidInfection = JsonSerializer.Deserialize<CovidInfection>(streamReader.ReadToEnd());
            document = new {id = Guid.NewGuid(), sex = covidInfection.sex, location = covidInfection.location, date = covidInfection.date};
            logger.LogInformation($"Adding new row with following information: {covidInfection.sex}, {covidInfection.location}, {covidInfection.date}");
        }
    }
}
