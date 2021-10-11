using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Storage.Queue;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public static class PopulateSourceQueue
    {
        [FunctionName(nameof(PopulateSourceQueue))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            IBinder binder,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function {nameof(PopulateSourceQueue)} processed a request.");

            try
            {
                // put messages on the queue (enqueue operation)
                                
                var queueAttr = new QueueAttribute(RequeueSampleData.sourceQueueName);
                var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttr);
                var queueMessage = new CloudQueueMessage(RequeueSampleData.messageOnQueue);

                int numberOfMessagesToEnqueue = 42;
                for (int i = 0; i < numberOfMessagesToEnqueue; i++)
                {
                    await cloudQueue.AddMessageAsync(queueMessage);
                }

                return new OkObjectResult($"{numberOfMessagesToEnqueue:N0} messages were enqueued for processing.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error Messgae: {ex.Message}");                
            }            
        }
    }
}