using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Queues;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public static class RequeueUsingStatic
    {
        [FunctionName(nameof(RequeueUsingStatic))]
        public static async Task Run([TimerTrigger("0 0 10-20/2 * * Mon-Fri")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function {nameof(RequeueUsingStatic)} executed at: {DateTime.Now:dd-MMM-yyyy}");

            try
            {
                var azure_storage_Uri = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

                QueueClient sourceQueue = await RequeueExtensions.GetQueueClientAsync(azure_storage_Uri, RequeueSampleData.sourceQueueName);
                QueueClient poisonQueue = await RequeueExtensions.GetQueueClientAsync(azure_storage_Uri, RequeueSampleData.poisonQueueName);

                await RequeueExtensions.RequeueItems(poisonQueue, sourceQueue);
            }
            catch (Exception ex)
            {
                // throw; // NOTE: not sure how safe it is to throw a TimeTrigger Exception, but you can never go wrong logging it somewhere
                log.LogCritical($"C# Timer trigger function {nameof(RequeueUsingStatic)} failed at: {DateTime.Now:dd-MMM-yyyy}, Error Message: {ex.Message}");
            }
        }
    }
}